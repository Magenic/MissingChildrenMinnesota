using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MCM.Droid.Classic
{
    public class ConfigHelper
    {
        private List<KeyValuePair<string, string>> _appSettings;

        public ConfigHelper(string configXML)
        {
            _appSettings = new List<KeyValuePair<string, string>>();

            // Read the contents of our asset
            XDocument xdoc = XDocument.Parse(configXML);
            if (xdoc.Root.Name.LocalName.Equals("appSetting", StringComparison.OrdinalIgnoreCase))
            {
                var lvls = xdoc.Root.DescendantNodes();
                foreach (XElement lvl in lvls)
                {
                    string key = string.Empty;
                    string keyValue = string.Empty;

                    if (lvl.Name.ToString().Equals("add"))
                    {
                        if (!string.IsNullOrWhiteSpace(lvl.Attribute("key").Value))
                        {
                            key = lvl.Attribute("key").Value;
                            if (!string.IsNullOrWhiteSpace(lvl.Attribute("value").Value))
                            {
                                keyValue = lvl.Attribute("value").Value;
                            }
                            _appSettings.Add(new KeyValuePair<string, string>(key, keyValue));
                        }
                    }
                }
            }
        }

        public string AppSettings(string key)
        {
            var keySetting = _appSettings.Find(_ => _.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value;

            return (string.IsNullOrWhiteSpace(keySetting) ? string.Empty : keySetting); 
        }
    }
}