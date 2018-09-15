using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace BudgetForecast.Config
{
    public class ConfigurationCollection : ConfigurationElementCollection
    {
        public ConfigurationCollection this[int index]
        {
            get { return (ConfigurationCollection)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public Hashtable ToHashtable()
        {
            Hashtable returnedValue = new Hashtable();
            foreach (object key in this.BaseGetAllKeys())
            {
                PathElement current = (PathElement)BaseGet(key);
                returnedValue.Add(current.Host.ToUpperInvariant(), current.Value);
            }
            return returnedValue;
        }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> returnedValue = new Dictionary<string, string>();
            foreach (object key in this.BaseGetAllKeys())
            {
                PathElement current = (PathElement)BaseGet(key);
                if (current.Host.ToUpperInvariant() == System.Environment.MachineName.ToUpperInvariant())
                {
                    returnedValue.Add(current.Host, current.Value);
                }
            }
            return returnedValue;
        }

        public void Add(PathElement pathElement)
        {
            BaseAdd(pathElement);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PathElement)element).Host;
        }

        public void Remove(PathElement serviceConfig)
        {
            BaseRemove(serviceConfig.Host);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }

    //private string PropertyName;

    //public CollectionBase(string propertyName)
    //{
    //    this.PropertyName = propertyName;
    //}

    //public override ConfigurationElementCollectionType CollectionType
    //{
    //    get
    //    {
    //        return ConfigurationElementCollectionType.BasicMapAlternate;
    //    }
    //}
    //protected override string ElementName
    //{
    //    get
    //    {
    //        return PropertyName;
    //    }
    //}

    //protected override bool IsElementName(string elementName)
    //{
    //    return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
    //}

    //public override bool IsReadOnly()
    //{
    //    return false;
    //}

    //protected override ConfigurationElement CreateNewElement()
    //{
    //    return new PathElement();
    //}

    //protected override object GetElementKey(ConfigurationElement element)
    //{
    //    return ((PathElement)(element)).Name;
    //}
}
