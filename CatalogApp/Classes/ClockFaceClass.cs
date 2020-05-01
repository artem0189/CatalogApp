using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CatalogApp.Classes
{
    [Serializable]
    class ClockFaceClass : ElementAbstractClass
    {
        [DisplayName("Материал")]
        public MaterialEnum Material { get; set; }

        [DisplayName("Тип цифр")]
        public NumbersEnum NumbersType { get; set; }
    }
}
