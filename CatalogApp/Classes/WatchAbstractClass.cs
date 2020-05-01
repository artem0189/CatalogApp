using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CatalogApp.Classes
{
    [Serializable]
    public enum RatingEnum
    {
        OneStar,
        TwoStar,
        ThreeStar,
        FourStar,
        FiveStart
    }

    [Serializable]
    public enum ColorEnum
    {
        Black,
        White,
        Silver
    }

    [Serializable]
    public enum MaterialEnum
    {
        Metal,
        Plastic
    }

    [Serializable]
    public enum ShapeEnum
    {
        Circle,
        Rectangle,
        Triangle
    }

    [Serializable]
    public enum NumbersEnum
    {
        Arabic,
        Baton,
        Arrows
    }

    [Serializable]
    public enum MechanismEnum
    {
        Quartz,
        Mtchanical
    }

    [Serializable]
    public enum MountingEnum
    {
        Hinge,
        Glue
    }

    [Serializable]
    abstract class WatchAbstractClass : ElementAbstractClass
    {
        [DisplayName("Корпус")]
        public BodyClass Body { get; set; }

        [DisplayName("Циферблат")]
        public ClockFaceClass ClockFace { get; set; }

        [DisplayName("Механизм")]
        public MechanismClass Mechanism { get; set; }

        [DisplayName("Брэнд")]
        public BrandClass Brand { get; set; }

        [DisplayName("Цена")]
        public int Price { get; set; }
    }
}
