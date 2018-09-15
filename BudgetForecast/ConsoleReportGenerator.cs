using BudgetForecast.Entities;
using System;
using System.Collections.Generic;

namespace BudgetForecast
{
    internal class ConsoleReportGenerator : IConsoleReportGenerator
    {
        const int size = 20;

        public void PrintHeader()
        {
            const string underline = "--------------------";
            Console.WriteLine("{0}        {1}   {2}   {3}   {4}   {5}   {6}   {7}", "Date",
                                Align("Account"),
                                Align("Payee"),
                                Align("Category"),
                                Align("Subcategory"),
                                Align("Amount"),
                                Align("Total"),
                                Align("Notes"));
            Console.WriteLine("{1}  {0}   {0}   {0}   {0}   {0}   {0}   {0}", underline, "----------");
        }

        public decimal PrintLines(IList<Entry> soFar, decimal previousTotal = 0)
        {
            decimal plus = 0;
            decimal minus = 0;

            foreach (Entry currentEntry in soFar)
            {
                if (currentEntry.Type == "Withdrawal")
                {
                    previousTotal -= currentEntry.Ammount;
                    minus += currentEntry.Ammount;
                }
                else
                {
                    previousTotal += currentEntry.Ammount;
                    plus += currentEntry.Ammount;
                }

                switch (currentEntry.Status)
                {
                    //Following
                    case "F":
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    //Void
                    case "V":
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;

                    //Recurring transaction in the future
                    case null:
                    //Reconciled
                    case "R": 
                    default:
                        break;
                }
                
                PrintLine(currentEntry, previousTotal);
                Console.ResetColor();
            }

            return previousTotal;
        }

        private void PrintLine(Entry currentEntry, decimal total)
        {
            Console.WriteLine("{0}  {1}   {2}   {3}   {4}   {5}   {6}   {7}", currentEntry.Moment.ToShortDateString(),
                                Align(currentEntry.Source),
                                Align(currentEntry.Payee),
                                Align(currentEntry.Category),
                                Align(currentEntry.Subcategory),
                                RAlign(((currentEntry.Type == "Withdrawal") ? currentEntry.Ammount * -1 : currentEntry.Ammount)),
                                RAlign(total),
                                Align(currentEntry.Notes, 45));
        }        

        #region "Align methods for columns"
        private string Align(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.PadRight(size, ' ')
                    .Replace("\n", " ")
                    .Substring(0, size);
            else
                return " ".PadRight(size, ' ')
                    .Replace("\n", " ")
                    .Substring(0, size);
        }

        private string Align(string value, int customSize)
        {
            if (!string.IsNullOrEmpty(value))
                return value.PadRight(size, ' ')
                    .Replace("\n", " ")
                    .Substring(0, (customSize > value.Length) ? value.Length : customSize);
            else
                return " ".PadRight(size, ' ')
                    .Replace("\n", " ")
                    .Substring(0, (customSize > value.Length) ? value.Length : customSize);
        }

        private string RAlign(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.PadLeft(size, ' ').Substring(0, size);
            else
                return " ".PadLeft(size, ' ').Substring(0, size);
        }

        private string RAlign(decimal value)
        {
            return string.Format("{0:0.00}", value).PadLeft(size, ' ').Substring(0, size);
        }
        #endregion

    }
}
