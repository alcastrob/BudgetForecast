using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetForecast.Config
{
    public class PathElement : ConfigurationElement
    {
        public PathElement() { }

        public PathElement(string host, string value)
        {
            //this.Name = name;
            this.Host = host;
            this.Value = value;
        }

        [ConfigurationProperty("host", IsRequired = true, IsKey = true)]
        public string Host { get { return this["host"].ToString(); } set { this["host"] = value; } }

        [ConfigurationProperty("value", IsRequired = true, IsKey = false)]
        public string Value { get { return this["value"].ToString(); } set { this["value"] = value; } }
    }
}
