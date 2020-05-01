using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;

namespace CatalogApp.Serialization
{
    interface ISerialization
    {
        byte[] Serialize(Dictionary<string, BindingList<object>> elements);
        void Deserialize(byte[] data, ref Dictionary<string, BindingList<object>> elements);
    }
}
