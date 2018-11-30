using System;
using System.Xml.Serialization;

namespace Canvas
{
    //[XmlInclude(typeof(Circle)), XmlInclude(typeof(Line)), XmlInclude(typeof(Point)), XmlInclude(typeof(Rectangle))]  
    [XmlType("Figure")]
    public abstract class Figure : IDrawable
    {
        public abstract void Draw();
        public abstract bool Contains(Point p);
    }
}
