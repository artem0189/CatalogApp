using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace CatalogApp.Serialization
{
    [DisplayName("Пользовательская сериализация")]
    class CustomSerialization : ISerialization
    {
        private void ConvertObjectToString(object obj, ref string result)
        {
            result += obj.GetType().FullName + "{";
            foreach (var value in obj.GetType().GetProperties())
            {
                result += value.Name + "{";
                if (value.PropertyType.IsPrimitive || value.PropertyType.Equals(typeof(string)) || value.PropertyType.IsEnum)
                {
                    result += value.GetValue(obj);
                }
                else
                {
                    ConvertObjectToString(value.GetValue(obj), ref result);
                }
                result += "}";
            }
            result += "}";
        }

        public byte[] Serialize(Dictionary<string, BindingList<object>> elements)
        {
            string result = "";
            foreach (var value in elements)
            {
                result += value.Key + ":{";
                foreach (var obj in value.Value)
                {
                    ConvertObjectToString(obj, ref result);
                }
                result += "};";
            }
            result = result.Remove(result.Length - 1);
            return Encoding.UTF8.GetBytes(result);
        }

        private void FillObject(ref object obj, Dictionary<string, string> dictionary)
        {
            BindingList<object> list = new BindingList<object>();
            var type = obj.GetType();
            foreach (var value in dictionary)
            {
                PropertyInfo propertyInfo = type.GetProperty(value.Key.Substring(0, value.Key.IndexOf('|')));
                if (value.Value.IndexOf('{') != value.Value.LastIndexOf('{'))
                {
                    propertyInfo.SetValue(obj, ConvertStringToObject(value.Value, ref list));
                }
                else
                {
                    var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                    propertyInfo.SetValue(obj, converter.ConvertFrom(value.Value.Substring(1, value.Value.Length - 2)));
                }
            }
        }

        private Dictionary<string, string> GetListOfObjects(string content)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            content = content.Substring(1, content.Length - 2);
            int countOfBrackets = 0;
            string obj = "";
            for(int i = 0; i < content.Length; i++)
            {
                obj += content[i];
                switch (content[i])
                {
                    case '{':
                        countOfBrackets++;
                        break;
                    case '}':
                        countOfBrackets--;
                        break;
                }
                if (content[i] == '}' && countOfBrackets == 0)
                {
                    dictionary.Add(obj.Substring(0, obj.IndexOf('{')) + "|" + dictionary.Count, obj.Substring(obj.IndexOf('{')));
                    obj = "";
                }
            }
            
            return dictionary;
        }

        private object ConvertStringToObject(string content, ref BindingList<object> list)
        {
            object newObject = null;
            var objects = GetListOfObjects(content);
            foreach (var obj in objects)
            {
                newObject = Activator.CreateInstance(Type.GetType(obj.Key.Substring(0, obj.Key.IndexOf('|')), false, true));
                var properties = GetListOfObjects(obj.Value);
                FillObject(ref newObject, properties);
                list.Add(newObject);
            }
            return newObject;
        }

        private BindingList<object> ConvertStringToList(string content)
        {
            BindingList<object> list = new BindingList<object>();
            ConvertStringToObject(content, ref list);
            return list;
        }

        public void Deserialize(byte[] data, ref Dictionary<string, BindingList<object>> elements)
        {
            try
            {
                Dictionary<string, BindingList<object>> newDictionary = new Dictionary<string, BindingList<object>>();
                var lines = Encoding.UTF8.GetString(data).Split(';');
                foreach (var value in lines)
                {
                    string key = value.Substring(0, value.IndexOf(':'));
                    string objects = value.Substring(key.Length + 1);
                    newDictionary.Add(key, ConvertStringToList(objects));
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
