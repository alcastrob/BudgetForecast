using System;
using NDesk.Options;
using Ninject;
using System.Reflection;

namespace BudgetForecast
{
    class Program
    {
        static bool showHelp = false;
        static bool allTransactions = false;
        static int month = DateTime.Now.Month, year = DateTime.Now.Year;

        static void Main(string[] args)
        {
            try
            {
                bool shouldExit = ParseParameters(args);

                if (!shouldExit)
                {
                    var kernel = new StandardKernel();
                    kernel.Load(Assembly.GetExecutingAssembly());
                    var manager = kernel.Get<IBudgetManager>();

                    ConsoleKey keyPressed = ConsoleKey.Enter;
                    while (keyPressed != ConsoleKey.Escape)
                    {                        
                        DateTime time;
                        switch (keyPressed)
                        {
                            case ConsoleKey.LeftArrow:
                                time = DateTime.Now;
                                manager.DoCalculation(time.Month, time.Year, allTransactions);
                                break;
                            case ConsoleKey.RightArrow:
                                time = DateTime.Now.AddMonths(1);
                                manager.DoCalculation(time.Month, time.Year, allTransactions);
                                break;
                            default:
                                manager.DoCalculation(month, year, allTransactions);
                                break;
                        }
                        keyPressed = Console.ReadKey().Key;
                    }
                }
            }
            catch (OptionException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine("Try '" + System.AppDomain.CurrentDomain.FriendlyName + " --help' for more information.");
                return;
            }
            catch (SQLite.SQLiteException ex)
            {
                if (ex.Result == SQLite.SQLite3.Result.CannotOpen)
                {
                    Console.Error.WriteLine("The database cannot be opened");
                    Console.ReadLine();
                }
                else
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }

        private static bool ParseParameters(string[] args)
        {
            var p = new OptionSet()
                {
                    {"h|help", "show this message and exit", v=> showHelp = v!=null },
                    {"y=", "specify the year", (int v) => year = v},
                    {"m=", "specify the month", (int v) => month = v},
                    {"all", "take into account all the transactions, and not only the validated ones", v => allTransactions = (v!=null) }
                };

            p.Parse(args);

            if (showHelp)
            {
                p.WriteOptionDescriptions(Console.Out);                
            }
            return showHelp;
        }
    }
}
