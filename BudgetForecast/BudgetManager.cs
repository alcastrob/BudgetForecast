using System;
using System.Collections.Generic;
using SQLite;
using BudgetForecast.Entities;
using System.Text;
using System.Linq;

namespace BudgetForecast
{
    internal class BudgetManager : IBudgetManager
    {
        private readonly IConsoleReportGenerator reportGenerator;
        private readonly IDataManager dbManager;

        public BudgetManager(IConsoleReportGenerator reportGenerator, IDataManager dataManager)
        {
            this.reportGenerator = reportGenerator;
            this.dbManager = dataManager;
        }

        public void DoCalculation(int month, int year, bool allTransactions)
        {
            decimal total = 0;

            Console.SetWindowSize(200, 50);
            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();
            reportGenerator.PrintHeader();
            List<Entry> entriesSoFar = dbManager.GetMonthlyEntries(month, year, allTransactions);
            total = reportGenerator.PrintLines(entriesSoFar);
            List<Entry> pendingRecurring = dbManager.GetMonthlyRecurringEntries(month, year);
            var previousMonth = from x in pendingRecurring
                                where x.Moment < new DateTime(year, month, 1)
                                select x;
            List<Entry> recurring = new List<Entry>();
            recurring.AddRange(from x in pendingRecurring
                               where x.Moment >= new DateTime(year, month, 1)
                               select x);
            foreach (Entry entry in previousMonth)
            {
                if (WillBeRecurredThisMonth(entry, month, year))
                {
                    entry.Moment = entry.Moment.AddMonths(1);
                    recurring.Add(entry);
                }
            }

            if (pendingRecurring.Count != 0)
            {
                Console.Write("#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-");
                Console.Write("#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-");
                Console.Write("#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-");
                Console.Write("#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-");
                Console.Write("#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-");
                Console.Write("#-#-#-#-#-#-#-#-#-#-#-#");
                Console.WriteLine();
                reportGenerator.PrintLines(recurring.OrderBy(entry => entry.Moment).ToList(), total);
            }
        }

        private bool WillBeRecurredThisMonth(Entry entry, int month, int year)
        {
            switch (entry.Repeats)
            {
                case 3: //Monthly
                case 103:
                case 203:
                    return true;
                case 0: //None    
                case 4: //Bimonthly
                case 5: //Every 3 months
                case 6: //Every 6 months
                case 7: //Yearly
                case 8: //Every 4 months
                default:
                    return false;
            }
        }
    }
}
