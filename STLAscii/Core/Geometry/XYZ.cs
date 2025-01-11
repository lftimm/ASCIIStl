using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Core.Geometry
{
    public class XYZ(float x, float y, float z)
    {
        public float X { get; set; } = x;
        public float Y { get; set; } = y;
        public float Z { get; set; } = z;

        // Public Methods
        public override string ToString()
        {
            return $"({X};{Y};{Z})";
        }

        public Vector ToVector()
        {
            return new Vector(X, Y, Z);
        }


        // Private methods

    }
}
