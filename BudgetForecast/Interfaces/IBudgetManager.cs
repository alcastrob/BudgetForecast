namespace BudgetForecast
{
    internal interface IBudgetManager
    {
        void DoCalculation(int month, int year, bool allTransactions);
    }
}