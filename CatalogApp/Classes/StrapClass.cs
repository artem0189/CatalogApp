using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CatalogApp.Classes
{
    [Serializable]
    class StrapClass : ElementAbstractClass
    {
        [DisplayName("Длина")]
        public int Length { get; set; }

        [DisplayName("Цвет")]
        public ColorEnum Color { get; set; }

        [DisplayName("Материал")]
        public MaterialEnum Material { get; set; }
    }
}
