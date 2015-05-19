using System;
using System.IO;
using System.Xml.Linq;

namespace MCM.Core.Helpers
{
	// The concept for this class came from: https://www.sellsbrothers.com/Posts/Details/13738
    public class Configuration : IConfiguration
    {
		public T GetValue<T>(string key)
		{
			var type = this.GetType();
			var resource = string.Format("{0}.config.xml", type.Namespace).Replace(".Helpers", string.Empty);
			using (var stream = type.Assembly.GetManifestResourceStream(resource))
			{
				using (var reader = new StreamReader(stream))
				{
					var doc = XDocument.Parse(reader.ReadToEnd());
					var value = doc.Element("config").Element(key).Value;

					return (T)Convert.ChangeType(value, typeof(T));
				}
			}
		}
    }
}
