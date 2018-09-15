using System.Collections.Generic;
using BudgetForecast.Entities;

namespace BudgetForecast
{
    public interface IDataManager
    {
        List<Entry> GetMonthlyEntries(int month, int year, bool allTransactions);
        List<Entry> GetMonthlyRecurringEntries(int month, int year);
    }
}