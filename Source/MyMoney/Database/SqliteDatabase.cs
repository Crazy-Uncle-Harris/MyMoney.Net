﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using Walkabout.Utilities;
using System.Reflection;
using System.Data.Common;
using System.Data.SQLite;
using System.Data.SqlTypes;

namespace Walkabout.Data
{
    public class SqliteDatabase : SqlServerDatabase
    {
        public SqliteDatabase() 
        {
        }

        /// <summary>
        /// Return true if SQL CE is installed.
        /// </summary>
        public static bool IsSqliteInstalled
        {
            get
            {
                return true;
            }
        }

        public static string OfficialSqliteFileExtension = ".myMoney.db";

        private SQLiteConnection sqliteConnection;


        // bugbug: there's some sort of horrible exponential performance bug in the System.Data.Sqlite wrappers.
        // so for now we have to return false, even though that too is slow.  
        public override bool SupportsBatchUpdate { get { return false; } }

        protected override string GetConnectionString(bool includeDatabase)
        {
            string cstr = "Data Source=" + this.DatabasePath;            
            if (!string.IsNullOrEmpty(this.Password))
            {
                cstr += ";Password=" + this.Password;
            }
            return cstr;
        }

        public override string GetDatabaseFullPath()
        {
            return this.DatabasePath;
        }

        public override void Create()
        {
            string connectionString = GetConnectionString(true);

            if (!File.Exists(this.DatabasePath))
            {
                // nothing to do here, the create is implicit in the first Open of the database file.
            }   
        }

        public override void Delete()
        {
            Disconnect();
            if (File.Exists(this.DatabasePath))
            {
                File.Delete(this.DatabasePath);
            }
        }

        public static SqliteDatabase Restore(string backup, string databaseFile, string password)
        {
            string fullBackupPath = Path.GetFullPath(backup);
            string fullDatabasePath = Path.GetFullPath(databaseFile);

            var result = new SqliteDatabase()
            {
                DatabasePath = fullBackupPath,
                Password = password
            };

            result.Connect(); // make sure we can connect to it.
            result.Disconnect();

            // Ok, then we're good to copy it.
            File.Copy(fullBackupPath, fullDatabasePath, true);

            return new SqliteDatabase()
            {
                DatabasePath = fullDatabasePath,
                Password = password,
                BackupPath = fullBackupPath
            };
        }

        public override DbFlavor DbFlavor
        {
            get { return Data.DbFlavor.Sqlite; }
        }

        public override bool UpgradeRequired
        {
            get
            {
                return false;
            }
        }

        public override void Upgrade()
        {
            // TBD
        }

        public override DbConnection Connect()
        {
            if (this.sqliteConnection == null || this.sqliteConnection.State != ConnectionState.Open)
            {
                string constr = GetConnectionString(true);
                this.sqliteConnection = new SQLiteConnection(constr);
                this.sqliteConnection.Open();
            }
            return this.sqliteConnection;
        }

        public override void Disconnect()
        {
            if (this.sqliteConnection != null && this.sqliteConnection.State == ConnectionState.Open)
            {
                try
                {
                    using (this.sqliteConnection)
                    {
                        this.sqliteConnection.Close();
                    }
                }
                catch { }
            }
        }

        public override bool Exists
        {
            get
            {
                return File.Exists(this.DatabasePath);
            }
        }

        public override bool TableExists(string name)
        {
            //object result = ExecuteScalar("select * from INFORMATION_SCHEMA.tables where table_name = '" + mapping.TableName + "'");
            object result = ExecuteScalar("SELECT tbl_name FROM sqlite_master where tbl_name='" + name + "'");
            return (result != null);
        }


        /// <summary>
        /// Get the schema of the given table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Returns the list of columns found</returns>
        public override List<ColumnMapping> GetTableSchema(string tableName)
        {
            // get the CREATE TABLE statement, the second and subsequent lines are the column information.
            string sql = ExecuteScalar("select sql from sqlite_master where tbl_name='" + tableName + "'").ToString();

            // replace comma column separator with newline.
            sql = sql.Replace(",", "\r\n");

            List<ColumnMapping> columns = new List<ColumnMapping>();

            // parse it to find the columns and their type infomration.
            using (StringReader reader = new StringReader(sql))
            {
                bool first = true;
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    if (!first)
                    {
                        line = line.Trim();
                        if (line == ")")
                        {
                            // done
                            break;
                        }
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }


                        ColumnMapping c = ParseColumnSql(line.TrimEnd(new char[] { ' ', '\t', '\r', '\n', ',' }));
                        if (c != null)
                        {
                            columns.Add(c);
                        }
                    }
                    first = false;
                }
            }

            return columns;
        }

        /// <summary>
        /// Parse a line of SQL that creates a column and return the mapping for it.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>A ColumnMapping</returns>
        private ColumnMapping ParseColumnSql(string line)
        {
            // eg:   [Id] int NOT NULL,

            ColumnMapping result = new ColumnMapping();
            result.AllowNulls = true;

            int state = 0;

            for (int i = 0, n = line.Length; i < n; i++)
            {
                string id = null;
                char ch = line[i];
                if (char.IsWhiteSpace(ch))
                {
                    // skip whitespace.
                    continue;
                }

                if (ch == '[')
                {
                    // scan for closing bracket.
                    int j = line.IndexOf(']', i);
                    if (j < 0)
                    {
                        return null;
                    }
                    id = line.Substring(i + 1, j - i - 1);
                    i = j;
                }
                else if (state == 2)
                {
                    // take rest of line then.
                    id = line.Substring(i, n - i);
                    i = n;
                }
                else 
                {
                    int j = line.IndexOfAny(new char[] { ' ', '\t', '\r', '\n' }, i);
                    if (j < 0)
                    {
                        // take rest of line then.
                        id = line.Substring(i, n - i);
                        i = n;
                    }
                    else
                    {
                        id = line.Substring(i, j - i);
                        i = j;
                    }
                }

                id = id.Trim();
                if (state == 0) 
                {
                    result.ColumnName = id;
                    state++;
                }
                else if (state == 1)
                {
                    // ok, now we have a sql data type.
                    ParseSqlType(id, result);
                    state++;
                }
                else
                {
                    // NOT NULL
                    // PRIMARY KEY
                    if (id.ToUpperInvariant().Contains("NOT NULL"))
                    {
                        result.AllowNulls = false;
                    }
                    if (id.ToUpperInvariant().Contains("PRIMARY KEY"))
                    {
                        result.IsPrimaryKey = true;
                        result.AllowNulls = false;
                    }
                }
            }
            return result;
        }

        private void ParseSqlType(string type, ColumnMapping result)
        {
            // split nvarchar(20) into [nvarchar][20].
            // split decimal(8,12) into [decimal[9][12]
            string[] parts = type.Split('(',',',')');
            Type columnType = null;
            bool hasLength = false;
            bool hasPrecision = false;
            switch (parts[0])
            {
                case "int":
                case "integer":
                case "numeric":
                    columnType = typeof(SqlInt32);
                    break;
                case "char":
                    columnType =  typeof(SqlAscii);
                    hasLength = true;
                    break;
                case "nchar":
                case "nvarchar":
                    columnType =  typeof(SqlChars);
                    hasLength = true;
                    break;
                case "money":
                    columnType =  typeof(SqlMoney);
                    hasPrecision = true;
                    break;
                case "datetime":
                    columnType =  typeof(SqlDateTime);
                    break;
                case "uniqueidentifier":
                    columnType =  typeof(SqlGuid);
                    break;
                case "decimal":
                    columnType =  typeof(SqlDecimal);
                    hasPrecision = true;
                    break;
                case "bigint":
                    columnType =  typeof(SqlInt64);
                    break;
                case "smallint":
                    columnType =  typeof(SqlInt16);
                    break;
                case "tinyint":
                    columnType =  typeof(SqlByte);
                    break;
                case "float":
                    columnType =  typeof(SqlSingle);
                    break;
                case "real":
                    columnType =  typeof(SqlDouble);
                    break;
                case "bit":
                    columnType =  typeof(SqlBoolean);
                    break;
                default:
                    throw new NotImplementedException(string.Format("SQL type '{0}' is not supported by the mapping engine", parts[0]));
            }
            result.SqlType = columnType;
            
            if (hasLength)
            {
                if (parts.Length > 1)
                {
                    int len = 0;
                    if (int.TryParse(parts[1], out len))
                    {
                        result.MaxLength = len;
                    }
                }
            }
            else if (hasPrecision)
            {
                if (parts.Length > 1)
                {
                    int precision = 0;
                    if (int.TryParse(parts[1], out precision))
                    {
                        result.Precision = precision;
                    }
                }
                if (parts.Length > 2)
                {
                    int scale = 0;
                    if (int.TryParse(parts[2], out scale))
                    {
                        result.Scale = scale;
                    }
                }
            }

            
        }

        public override object ExecuteScalar(string cmd)
        {
            Debug.Assert(this.DbFlavor == DbFlavor.Sqlite);

            if (cmd == null || cmd.Trim().Length == 0) return null;

            this.AppendLog(cmd);
            object result = null;
            try
            {
                Connect();
                using (DbCommand command = new SQLiteCommand(cmd, this.sqliteConnection))
                {
                    result = command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error executing SQL \"" + cmd + "\"\n" + ex.Message);
            }
            return result;
        }

        public override DataSet QueryDataSet(string query)
        {
            Debug.Assert(this.DbFlavor == DbFlavor.Sqlite);

            if (string.IsNullOrWhiteSpace(query))
            {
                return null;
            }

            try
            {
                Connect();

                DataSet dataSet = new DataSet();

                using (DbDataAdapter da = new SQLiteDataAdapter(query, this.sqliteConnection))
                {
                    da.Fill(dataSet, "Results");
                }

                if (dataSet.Tables.Contains("Results"))
                {
                    return dataSet;
                }

            }
            catch (Exception)
            {
                throw; // useful for setting breakpoints.
            }
            return null;
        }

        public override void ExecuteNonQuery(string cmd)
        {
            Debug.Assert(this.DbFlavor == DbFlavor.Sqlite);
            if (cmd == null || cmd.Trim().Length == 0) return;

            this.AppendLog(cmd);
            try
            {
                Connect();
                using (DbCommand command = new SQLiteCommand(cmd, this.sqliteConnection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw; // useful for setting breakpoints.
            }
        }

        public override IDataReader ExecuteReader(string cmd)
        {
            Debug.Assert(this.DbFlavor == DbFlavor.Sqlite);

            this.AppendLog(cmd); 

            try
            {
                Connect();
                using (DbCommand command = new SQLiteCommand(cmd, this.sqliteConnection))
                {
                    return command.ExecuteReader();
                }
            }
            catch (Exception)
            {
                throw; // useful for setting breakpoints.
            }
        }


        public override void Backup(string backupPath)
        {
            this.BackupPath = backupPath;
            File.Copy(this.DatabasePath, backupPath);
        }

    }
}
