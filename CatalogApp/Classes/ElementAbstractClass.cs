using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CatalogApp.Classes
{
    [Serializable]
    abstract class ElementAbstractClass
    {
        [DisplayName("Наименование")]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
