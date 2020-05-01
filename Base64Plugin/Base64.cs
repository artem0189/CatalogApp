using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;
using System.IO;

namespace Base64Plugin
{
    public class Base64 : IPlugin
    {
        public string Name { get; } = "Base64";
        public string Extension { get; } = ".base64";
        public byte[] OnSave(byte[] data)
        {
            return Encoding.UTF8.GetBytes(Convert.ToBase64String(data));
        }

        public byte[] OnLoad(byte[] data)
        {
            return Convert.FromBase64String(Encoding.UTF8.GetString(data));
        }
    }
}
