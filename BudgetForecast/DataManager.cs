using BudgetForecast.Config;
using BudgetForecast.Entities;
using SQLite;
using System;
using System.Collections.Generic;

namespace BudgetForecast
{
    internal class DataManager : IDataManager
    {
        private SQLiteConnection connection;
      
        public DataManager(IConfigManager config)
        {
            this.connection = new SQLiteConnection(config.pathConfig.Path);
        }

        public List<Entry> GetMonthlyEntries(int month, int year, bool allTransactions)
        {
            DateTime begining = new DateTime(year, month, 1);
            DateTime ending = begining.AddMonths(1).AddDays(-1);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(@"SELECT a.ACCOUNTNAME AS Source, a1.ACCOUNTNAME AS Destination, ");
            sb.AppendLine(@"p.PAYEENAME AS Payee, c.TRANSCODE AS Type, c.NOTES AS Notes, cat.CATEGNAME AS Category, ");
            sb.AppendLine(@"cat2.SUBCATEGNAME AS Subcategory, c.TRANSDATE Moment, c.TOTRANSAMOUNT AS Ammount, ");
            sb.AppendLine(@"c.Status AS Status");
            sb.AppendLine(@"FROM CHECKINGACCOUNT_V1 c");
            sb.AppendLine(@"LEFT JOIN ACCOUNTLIST_V1 a ON c.ACCOUNTID =a.ACCOUNTID");
            sb.AppendLine(@"LEFT JOIN ACCOUNTLIST_V1 a1 ON c.TOACCOUNTID =a1.ACCOUNTID");
            sb.AppendLine(@"LEFT JOIN PAYEE_V1 p ON p.PAYEEID = c.PAYEEID");
            sb.AppendLine(@"LEFT JOIN CATEGORY_V1 cat ON cat.CATEGID = c.CATEGID");
            sb.AppendLine(@"LEFT JOIN SUBCATEGORY_V1 cat2 ON cat2.SUBCATEGID = c.SUBCATEGID");
            sb.AppendLine(@"WHERE c.TransDate >= ? ");
            sb.AppendLine(@"AND c.TransDate <= ?");
            sb.AppendLine(@"AND a.CurrencyId = 6"); //Euros
            if (!allTransactions)
            {
                sb.AppendLine(@"AND c.Status = 'R'");
            }
            sb.AppendLine(@"AND TRANSCODE != 'Transfer'");
            sb.AppendLine(@"ORDER BY TransDate");            

            return connection.Query<Entry>(sb.ToString(), new object[] { begining.ToString("yyyy-MM-dd"), ending.ToString("yyyy-MM-dd")});
        }

        public List<Entry> GetMonthlyRecurringEntries(int month, int year)
        {
            DateTime begining = new DateTime(year, month, 1);
            DateTime ending = begining.AddMonths(1).AddDays(-1);
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(@"SELECT a.ACCOUNTNAME AS Source, a1.ACCOUNTNAME AS Destination, ");
            sb.AppendLine(@"p.PAYEENAME AS Payee, c.TRANSCODE AS Type, c.NOTES AS Notes, cat.CATEGNAME AS Category, ");
            sb.AppendLine(@"cat2.SUBCATEGNAME AS Subcategory, c.TRANSDATE Moment, c.TOTRANSAMOUNT AS Ammount, ");
            sb.AppendLine(@"c.REPEATS AS Repeats");
            sb.AppendLine(@"FROM BILLSDEPOSITS_V1 c");
            sb.AppendLine(@"LEFT JOIN ACCOUNTLIST_V1 a ON c.ACCOUNTID =a.ACCOUNTID");
            sb.AppendLine(@"LEFT JOIN ACCOUNTLIST_V1 a1 ON c.TOACCOUNTID =a1.ACCOUNTID");
            sb.AppendLine(@"LEFT JOIN PAYEE_V1 p ON p.PAYEEID = c.PAYEEID");
            sb.AppendLine(@"LEFT JOIN CATEGORY_V1 cat ON cat.CATEGID = c.CATEGID");
            sb.AppendLine(@"LEFT JOIN SUBCATEGORY_V1 cat2 ON cat2.SUBCATEGID = c.SUBCATEGID");
            sb.AppendLine(@"WHERE TRANSCODE != 'Transfer'");
            sb.AppendLine(@"AND c.TransDate <= ?");
            sb.AppendLine(@"AND a.CurrencyId = 6"); //Euros
            sb.AppendLine(@"ORDER BY NEXTOCCURRENCEDATE");

            return connection.Query<Entry>(sb.ToString(), new object[] { ending });
        }
    }
}
