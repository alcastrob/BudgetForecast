using BudgetForecast.Config;
using Ninject.Modules;

namespace BudgetForecast
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IConsoleReportGenerator>().To<ConsoleReportGenerator>();
            Bind<IBudgetManager>().To<BudgetManager>();
            Bind<IConfigManager>().To<ConfigManager>();
            Bind<IPathConfig>().To<PathConfig>();
            Bind<IDataManager>().To<DataManager>();         
        }
    }
}
