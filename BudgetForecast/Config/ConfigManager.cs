using System.Collections;
using System.Configuration;

namespace BudgetForecast.Config
{
    internal class ConfigManager : IConfigManager
    {
        public IPathConfig pathConfig { get; set; }
        
        public ConfigManager(IPathConfig pathConfig)
        {
            PathSection configSection = System.Configuration.ConfigurationManager.GetSection("pathsSection") as PathSection;
            this.pathConfig = pathConfig;
            string env = System.Environment.MachineName.ToUpperInvariant();
            Hashtable paths = configSection.Paths.ToHashtable();
            if (!paths.ContainsKey(env))
                throw new ConfigurationErrorsException("Missing environment " + env);
            this.pathConfig.Path = paths[env].ToString();
        }
    }

    public class PathConfig : IPathConfig
    {
        public string Path { get; set; }
    }
}
