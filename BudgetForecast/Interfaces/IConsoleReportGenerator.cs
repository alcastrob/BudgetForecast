using System.Collections.Generic;
using BudgetForecast.Entities;

namespace BudgetForecast
{
    internal interface IConsoleReportGenerator
    {
        void PrintHeader();
        decimal PrintLines(IList<Entry> soFar, decimal previousTotal = 0);
    }
}