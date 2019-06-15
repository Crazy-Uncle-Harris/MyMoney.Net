﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Walkabout.Data;
using Walkabout.Utilities;

namespace Walkabout.Network
{
    class IEXTrading : IStockQuoteService
    {
        static string FriendlyName = "iexcloud.io";
        // See https://iextrading.com/developer/docs/#batch-requests
        const string address = "https://cloud.iexapis.com/stable/stock/market/batch?symbols={0}&types=quote&range=1m&last=1&token={1}";
        char[] illegalUrlChars = new char[] { ' ', '\t', '\n', '\r', '/', '+', '=', '&', ':' };
        StockServiceSettings _settings;
        List<Security> _pending;
        Thread _downloadThread;
        HttpWebRequest _current;
        bool _cancelled;

        public IEXTrading(StockServiceSettings settings)
        {
            _settings = settings;
            settings.Name = FriendlyName;
        }

        public int PendingCount { get { return (_pending == null) ? 0 : _pending.Count; } }

        public void Cancel()
        {
            _cancelled = true;
            if (_current != null)
            {
                try
                {
                    _current.Abort();
                }
                catch { }
            }
        }

        public event EventHandler<StockQuote> QuoteAvailable;

        private void OnQuoteAvailable(StockQuote quote)
        {
            if (QuoteAvailable != null)
            {
                QuoteAvailable(this, quote);
            }
        }

        public event EventHandler<string> DownloadError;

        private void OnError(string message)
        {
            if (DownloadError != null)
            {
                DownloadError(this, message);
            }
        }

        public event EventHandler<bool> Complete;

        private void OnComplete(bool complete)
        {
            if (Complete != null)
            {
                Complete(this, complete);
            }
        }

        public static StockServiceSettings GetDefaultSettings()
        {
            return new StockServiceSettings()
            {
                Name = FriendlyName,
                ApiKey = "",
                ApiRequestsPerMinuteLimit = 60,
                ApiRequestsPerDayLimit = 0,
                ApiRequestsPerMonthLimit = 500000
            };
        }

        public static bool IsMySettings(StockServiceSettings settings)
        {
            return settings.Name == FriendlyName;
        }

        public void BeginFetchQuotes(List<Security> securities)
        {
            int count = 0;
            if (_pending == null)
            {
                _pending = securities;
                count = securities.Count;
            }
            else
            {
                lock (_pending)
                {
                    // merge the lists.
                    foreach (Security s in securities)
                    {
                        if (!(from p in _pending where p.Symbol == s.Symbol select p).Any())
                        {
                            _pending.Add(s);
                        }
                    }
                    count = _pending.Count;
                }
            }
            _cancelled = false;
            if (_downloadThread == null)
            {
                _downloadThread = new Thread(new ThreadStart(DownloadQuotes));
                _downloadThread.Start();
            }
        }

        private void DownloadQuotes()
        {
            try
            {
                StockQuoteThrottle.Instance.Settings = this._settings;
                // This is on a background thread
                int max_batch = 100;
                List<Security> batch = new List<Security>();
                while (!_cancelled)
                {
                    int remaining = 0;
                    Security security = null;
                    lock (_pending)
                    {
                        if (_pending.Count > 0)
                        {
                            security = _pending[0];
                            _pending.RemoveAt(0);
                            remaining = _pending.Count;
                        }
                    }
                    if (security == null)
                    {
                        // done!
                        break;
                    }

                    // weed out any securities that have no symbol or have a symbol that would be invalid.               
                    string symbol = security.Symbol;
                    if (string.IsNullOrEmpty(symbol))
                    {
                        // skip securities that have no symbol.
                    }
                    else if (symbol.IndexOfAny(illegalUrlChars) >= 0)
                    {
                        // since we are passing the symbol on an HTTP URI line, we can't pass Uri illegal characters...
                        OnError(string.Format(Walkabout.Properties.Resources.SkippingSecurityIllegalSymbol, symbol));
                    }
                    else
                    {
                        batch.Add(security);
                    }

                    if (batch.Count() == max_batch || remaining == 0)
                    {
                        string symbols = string.Join(",", from s in batch select s.Symbol);
                        try
                        {
                            string uri = string.Format(address, symbols, _settings.ApiKey);
                            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                            req.UserAgent = "USER_AGENT=Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1;)";
                            req.Method = "GET";
                            req.Timeout = 10000;
                            req.UseDefaultCredentials = false;
                            _current = req;

                            WebResponse resp = req.GetResponse();
                            using (Stream stm = resp.GetResponseStream())
                            {
                                using (StreamReader sr = new StreamReader(stm, Encoding.UTF8))
                                {
                                    string json = sr.ReadToEnd();
                                    JObject o = JObject.Parse(json);
                                    List<StockQuote> result = ParseStockQuotes(o);
                                    foreach (Security s in batch)
                                    {
                                        StockQuote q = (from i in result where string.Compare(i.Symbol, s.Symbol, StringComparison.OrdinalIgnoreCase) == 0 select i).FirstOrDefault();
                                        if (q == null)
                                        {
                                            OnError(string.Format("No quote returned for symbol {0}", s.Symbol));
                                        }
                                        else
                                        {
                                            OnQuoteAvailable(q);
                                        }
                                    }
                                }
                            }

                            // this service doesn't want too many calls per second.
                            int ms = StockQuoteThrottle.Instance.GetSleep();
                            while (!_cancelled)
                            {
                                Thread.Sleep(1000);
                                ms -= 1000;
                                if (ms <= 0)
                                {
                                    break;
                                }
                            }
                        }
                        catch (System.Net.WebException we)
                        {
                            if (we.Status != WebExceptionStatus.RequestCanceled)
                            {
                                OnError(string.Format(Walkabout.Properties.Resources.ErrorFetchingSymbols, symbols) + "\r\n" + we.Message);
                            }
                            else
                            {
                                // we cancelled, so bail. 
                                _cancelled = true;
                                break;
                            }

                            HttpWebResponse http = we.Response as HttpWebResponse;
                            if (http != null)
                            {
                                // certain http error codes are fatal.
                                switch (http.StatusCode)
                                {
                                    case HttpStatusCode.ServiceUnavailable:
                                    case HttpStatusCode.InternalServerError:
                                    case HttpStatusCode.Unauthorized:
                                        OnError(http.StatusDescription);
                                        _cancelled = true;
                                        break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            // continue
                            OnError(string.Format(Walkabout.Properties.Resources.ErrorFetchingSymbols, symbols) + "\r\n" + e.Message);
                        }
                    }

                    _current = null;
                    OnComplete(remaining == 0);

                }
            }
            catch
            {
            }
            StockQuoteThrottle.Instance.Save();
            _downloadThread = null;
            _current = null;
        }


        private static List<StockQuote> ParseStockQuotes(JObject o)
        {
            List<StockQuote> result = new List<StockQuote>();
            // See https://iexcloud.io/docs/api/#quote, lots more info available than what we are extracting here.
            foreach (var pair in o)
            {
                // KeyValuePair<string, JToken>
                string name = pair.Key;
                JToken token = pair.Value;
                if (token.Type == JTokenType.Object)
                {
                    var quote = new StockQuote();
                    result.Add(quote);

                    JObject child = (JObject)token;
                    JToken value;
                    if (child.TryGetValue("quote", StringComparison.Ordinal, out value) && value.Type == JTokenType.Object)
                    {
                        child = (JObject)value;

                        if (child.TryGetValue("symbol", StringComparison.Ordinal, out value))
                        {
                            quote.Symbol = (string)value;
                        }
                        if (child.TryGetValue("companyName", StringComparison.Ordinal, out value))
                        {
                            quote.Name = (string)value;
                        }
                        if (child.TryGetValue("close", StringComparison.Ordinal, out value))
                        {
                            quote.Price = (decimal)value;
                        }
                        if (child.TryGetValue("closeTime", StringComparison.Ordinal, out value))
                        {
                            long ticks = (long)value;
                            quote.Date = DateTimeOffset.FromUnixTimeMilliseconds(ticks).LocalDateTime;
                        }
                    }
                }
            }
            return result;
        }

    }
}