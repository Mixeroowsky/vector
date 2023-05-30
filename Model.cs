using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vector_graphic_viewer
{
    public enum Type
    {
        line, circle, triangle
    }
    internal class Model
    {
        public Type Type { get; set; }
        public string a { get; set; }
        public string b { get; set; }
        public string c { get; set; }
        public string Color { get; set; }
        public string Center { get; set; }
        public double Radius { get; set; }
        public bool Filled { get; set; }
    }
}
