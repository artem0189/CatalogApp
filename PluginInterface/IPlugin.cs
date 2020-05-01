using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
    public interface IPlugin
    {
        string Name { get; }
        string Extension { get; }
        byte[] OnSave(byte[] data);
        byte[] OnLoad(byte[] data);
    }
}
