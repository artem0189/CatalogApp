using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Forms;

namespace CatalogApp.Serialization
{
    [DisplayName("JSON")]
    class JSONSerialization : ISerialization
    {
        public byte[] Serialize(Dictionary<string, BindingList<object>> elements)
        {
            string result = JsonConvert.SerializeObject(elements);
            return Encoding.UTF8.GetBytes(result);
        }

        public void Deserialize(byte[] data, ref Dictionary<string, BindingList<object>> elements)
        {
            try
            {
                Dictionary<string, BindingList<object>> newDictionary = new Dictionary<string, BindingList<object>>();
                newDictionary = JsonConvert.DeserializeObject<Dictionary<string, BindingList<object>>>(Encoding.UTF8.GetString(data));
                elements = newDictionary;

                for (int i = 0; i < elements.Values.Count; i++)
                {
                    for (int j = 0; j < elements.Values.ElementAt(i).Count; j++)
                    {
                        var jobject = (JObject)elements.Values.ElementAt(i)[j];
                        elements.Values.ElementAt(i)[j] = jobject.ToObject(Type.GetType(elements.Keys.ElementAt(i), false, true));
                    }
                }
            }
            catch
            {
                MessageBox.Show("Неправильный файл!");
            }
        }
    }
}
