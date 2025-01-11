using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Core.Geometry
{
    public class XYZ(double x, double y, double z)
    {
        public double X { get; set; } = x;
        public double Y { get; set; } = y;
        public double Z { get; set; } = z;

        // Public Methods
        public override string ToString()
        {
            return $"({X};{Y};{Z})";
        }

        public Vector ToVector()
        {
            return new Vector(X, Y, Z);
        }


        // Private mehtods

    }
}
