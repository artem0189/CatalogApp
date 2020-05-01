using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace CatalogApp.Serialization
{
    [DisplayName("Двоичная сериализация")]
    class BinarySerialization : ISerialization
    {
        public byte[] Serialize(Dictionary<string, BindingList<object>> elements)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, elements);
                stream.Position = 0;
                return stream.ToArray();
            }
        }

        public void Deserialize(byte[] data, ref Dictionary<string, BindingList<object>> elements)
        {
            try
            {
                Dictionary<string, BindingList<object>> newDictionary = new Dictionary<string, BindingList<object>>();
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream(data))
                {
                    newDictionary = (Dictionary<string, BindingList<object>>)formatter.Deserialize(stream);
                }
                elements = newDictionary;
            }
            catch
            {
                MessageBox.Show("Неправильный файл!");
            }
        }
    }
}
