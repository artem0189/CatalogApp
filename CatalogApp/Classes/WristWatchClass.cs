using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CatalogApp.Classes
{
    [Serializable]
    [DisplayName("Наручные часы")]
    class WristWatchClass : WatchAbstractClass
    {
        [DisplayName("Ремешок")]
        public StrapClass Strap { get; set; }
    }
}
