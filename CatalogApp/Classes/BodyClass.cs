using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CatalogApp.Classes
{
    [Serializable]
    class BodyClass : ElementAbstractClass
    {
        [DisplayName("Цвет")]
        public ColorEnum Color { get; set; }

        [DisplayName("Материал")]
        public MaterialEnum Material { get; set; }

        [DisplayName("Форма")]
        public ShapeEnum Shape { get; set; }
    }
}
