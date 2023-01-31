﻿using NUnit.Framework;
using Walkabout.Data;
using Walkabout.Taxes;
using Walkabout.Utilities;

namespace Walkabout.Tests
{
    public class CostBasisTests
    {
        [Test]
        public void SimpleCostBasis()
        {
            UiDispatcher.CurrentDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            MyMoney m = new MyMoney();
            Security s = m.Securities.NewSecurity();
            s.Name = "MSFT";
            Account a = m.Accounts.AddAccount("Checking");
            a.Type = AccountType.Brokerage;

            decimal lot1Price = 1;
            decimal lot1Units = 10;
            // start in 2000 with 10 units with a cost basis of $1 each (this is LOT 1)
            AddInvestment(m, s, a, DateTime.Parse("1/1/2000"), lot1Units, lot1Price, InvestmentType.Add);
            m.StockSplits.AddStockSplit(new StockSplit()
            {
                Date = DateTime.Parse("6/1/2000"),
                Numerator = 2,
                Denominator = 1,
                Security = s
            });
            // should now have 20 units in LOT 1
            decimal lot2Price = 2;
            // sell 10 units in 2001 from LOT 1, means LOT 1 has 10 units remaining.
            AddInvestment(m, s, a, DateTime.Parse("1/1/2001"), 10, lot2Price, InvestmentType.Sell);
            decimal lot2Units = 20;
            // buy 20 more at $2 in 2002 (this is LOT 2)
            AddInvestment(m, s, a, DateTime.Parse("1/1/2002"), lot2Units, lot2Price, InvestmentType.Buy);

            // should have a total of 30 units now, 10 in Lot 1 and 20 in Lot 2.

            // Record a stock split in June 2002
            m.StockSplits.AddStockSplit(new StockSplit()
            {
                Date = DateTime.Parse("6/1/2002"),
                Numerator = 3,
                Denominator = 1,
                Security = s
            });

            // Should now have 90 units
            // There should be 30 in LOT 1 and 60 in LOT 2.
            decimal lot3Price = 3;
            decimal lot3Units = 10;
            // Buy 10 more units in 2003 at $3, should now have 100 (This is LOT 3).
            AddInvestment(m, s, a, DateTime.Parse("1/1/2003"), lot3Units, lot3Price, InvestmentType.Buy);

            // Sell 50 units in 2004 at $3, this should sell in FIFO order, so LOT 1 is emptied of
            // it's 30 units, and LOT 2 loses the remainder of 20 units, so Lot 2 now has 40 units.
            // LOT 3 is untouched with 10 units.
            AddInvestment(m, s, a, DateTime.Parse("1/1/2004"), 50, lot3Price, InvestmentType.Sell);

            // Test remove that is not a transfer, again in FIFO order this drains 20 units from LOT 2
            // leaving 20 units there.
            AddInvestment(m, s, a, DateTime.Parse("1/1/2005"), 20, 5, InvestmentType.Remove);

            CapitalGainsTaxCalculator calc = new CapitalGainsTaxCalculator(m, DateTime.Now, false, false);
            List<SecurityPurchase> holdings = new List<SecurityPurchase>(calc.GetHolding(a).GetHoldings());

            Assert.AreEqual(2, holdings.Count); // should have only 2 buys left, the first buy is all used up.
            SecurityPurchase c1 = holdings[0];
            SecurityPurchase c2 = holdings[1];

            Assert.AreEqual(20, c1.UnitsRemaining); // 10 shares left Lot 2 (after stock splits)
            Assert.AreEqual(10, c2.UnitsRemaining); // 10 shares still in Lot 3 (has no stock spits)
            // a 3:1 split means new prices is 'price / 3' and original 
            Assert.AreEqual(Math.Round(lot2Price / 3M, 5), Math.Round(c1.CostBasisPerUnit, 5));
            Assert.AreEqual(Math.Round(lot3Price, 5), Math.Round(c2.CostBasisPerUnit, 5)); // no splits apply.

        }

        [Test]
        public void CostBasisAcrossTransfers()
        {
            UiDispatcher.CurrentDispatcher = System.Windows.Threading.Dispatcher.CurrentDispatcher;
            MyMoney m = new MyMoney();
            Security s = m.Securities.NewSecurity();
            s.Name = "MSFT";
            Account a = m.Accounts.AddAccount("Ameritrade");
            a.Type = AccountType.Brokerage;
            Account a2 = m.Accounts.AddAccount("Fidelity");
            a2.Type = AccountType.Brokerage;

            AddInvestment(m, s, a, DateTime.Parse("1/1/2000"), 10, 1, InvestmentType.Buy);
            m.StockSplits.AddStockSplit(new StockSplit()
            {
                Date = DateTime.Parse("6/1/2000"),
                Numerator = 2,
                Denominator = 1,
                Security = s
            });
            AddInvestment(m, s, a, DateTime.Parse("1/1/2001"), 20, 2, InvestmentType.Buy);

            Transaction transfer = AddInvestment(m, s, a, DateTime.Parse("1/1/2002"), 30, 2, InvestmentType.Remove);
            m.Transfer(transfer, a2);

            // now should be able to sell 10 left in this account (after split)
            AddInvestment(m, s, a, DateTime.Parse("1/1/2003"), 10, 3, InvestmentType.Sell);

            // and we should have 30 in the other account
            AddInvestment(m, s, a2, DateTime.Parse("1/1/2004"), 30, 5, InvestmentType.Sell);

            // Ok, now let's if the cost basis is correct!
            CostBasisCalculator calc = new CostBasisCalculator(m, DateTime.Now);
            List<SecurityPurchase> holdings = new List<SecurityPurchase>(calc.GetHolding(a).GetHoldings());

            Assert.AreEqual(0, holdings.Count); // should have nothing left.

            // should have 3 separate cost basis records to cover what we sold.
            List<SecuritySale> sales = new List<SecuritySale>(calc.GetSales());
            Assert.AreEqual(3, sales.Count);

            SecuritySale s1 = sales[0];
            SecuritySale s2 = sales[1];
            SecuritySale s3 = sales[2];

            // since the sale from Ameritrade account happened first it should be returned first
            Assert.AreEqual(2, s1.CostBasisPerUnit); // $2, no splits
            Assert.AreEqual(a, s1.Account); // Ameritrade
            Assert.AreEqual(Math.Round(10M, 5), Math.Round(s1.UnitsSold, 5));

            // Notice here that the Fidelity account inherited the cost basis records correctly
            // from the Ameritrade account as a result of the "Transfer" that happened above.
            Assert.AreEqual(Math.Round(1M / 2M, 5), Math.Round(s2.CostBasisPerUnit, 5)); // $1 after 2:1 split
            Assert.AreEqual(a2, s2.Account); // Fidelity
            Assert.AreEqual(Math.Round(20M, 5), Math.Round(s2.UnitsSold, 5));

            Assert.AreEqual(2, s3.CostBasisPerUnit); // $2, no splits
            Assert.AreEqual(a2, s2.Account); // Fidelity
            Assert.AreEqual(Math.Round(10M, 5), Math.Round(s3.UnitsSold, 5));

        }
        private static Transaction AddInvestment(MyMoney m, Security s, Account a, DateTime date, decimal units, decimal unitPrice, InvestmentType type)
        {
            Transaction t = m.Transactions.NewTransaction(a);
            t.Date = date;
            Investment i = t.GetOrCreateInvestment();
            i.Security = s;
            i.Units = units;
            i.UnitPrice = unitPrice;
            i.Type = type;
            m.Transactions.AddTransaction(t);
            return t;
        }


    }
}
