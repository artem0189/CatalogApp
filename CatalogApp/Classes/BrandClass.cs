using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CatalogApp.Classes
{
    [Serializable]
    class BrandClass : ElementAbstractClass
    {
        [DisplayName("Страна")]
        public string Country { get; set; }

        [DisplayName("Рейтинг")]
        public RatingEnum Rating { get; set; } 
    }
}
