using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CatalogApp.Classes
{
    [Serializable]
    [DisplayName("Настенные часы")]
    class WallClockClass : WatchAbstractClass
    {
        [DisplayName("Крепление")]
        public MountingEnum MountingMechanism { get; set; }

        [DisplayName("Вес")]
        public int Weight { get; set; }
    }
}
