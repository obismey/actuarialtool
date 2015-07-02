using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace obismey.actuarialtools.desktop.helpers
{
    public class DynamicSettings
    {
        private static ExpandoObject _Instance;

        public static object Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ExpandoObject();

                    if (System.IO.File.Exists("VisualConfig.xml"))
                    {
                        var GridLengthConverter = new System.Windows.GridLengthConverter();

                        foreach (var xelt in XElement.Load("VisualConfig.xml").Elements("Property"))
                        {
                            var dic = (System.Collections.Generic.IDictionary<string, object>)_Instance;

                            if (xelt.Attribute("Type").Value== "GridLength")
                            {
                                dic.Add(
                                    xelt.Attribute("Name").Value,
                                    GridLengthConverter.ConvertFromInvariantString(xelt.Attribute("Value").Value));
                            }
                        }
                    }

                    ((System.ComponentModel.INotifyPropertyChanged)_Instance).PropertyChanged += DynamicSettings_PropertyChanged;
                }

                return _Instance;
            }
        }

        private static void DynamicSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var xfile = new XElement("Data");

            foreach (var kvp in _Instance)
            {
                var xelt = new XElement("Property");
                xelt.SetAttributeValue("Name", kvp.Key);
                xelt.SetAttributeValue("Value", kvp.Value);
                xelt.SetAttributeValue("Type", kvp.Value.GetType().Name);

                xfile.Add(xelt);
            }

            xfile.Save("VisualConfig.xml");

        }

    }
}
