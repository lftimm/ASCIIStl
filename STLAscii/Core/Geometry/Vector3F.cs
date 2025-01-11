using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Core.Geometry
{
    public struct Vector3F
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        public Vector3F(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3F(double x, double y, double z)
        {
            X = (float)x;
            Y = (float)y;
            Z = (float)z;
        }
    }
}
