using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace BudgetForecast.Config
{
    public class PathSection : ConfigurationSection
    {
        [ConfigurationProperty("paths", IsRequired = true)]
        [ConfigurationCollection(typeof(ConfigurationCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ConfigurationCollection Paths
        {
            get
            {
                return (ConfigurationCollection)base["paths"];
            }
        }
    }
}