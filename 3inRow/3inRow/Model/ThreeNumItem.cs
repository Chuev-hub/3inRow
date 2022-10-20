using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3inRow.Model
{
    internal class ThreeNumItem
    {
        public bool IsHorizontal;
        public List<Point> p { get; set; }  = new List<Point>();
        public class Point {
            public int i { get; set; }
            public int j { get; set; }
        }

    }
}
