using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CatalogApp.Classes
{
    [Serializable]
    class MechanismClass : ElementAbstractClass
    {
        [DisplayName("Механизм")]
        public MechanismEnum Mechnism { get; set; }

        [DisplayName("Точность хода")]
        public int StrokeAccuracy { get; set; }
    }
}
