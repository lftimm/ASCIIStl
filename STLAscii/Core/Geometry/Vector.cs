using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Core.Geometry
{
    public class Vector(double x, double y, double z) : XYZ(x, y, z)
    {
        public double Length { get => GetLength(); }

        public double DotProduct(Vector v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        public Vector CrossProduct(Vector v)
        {
            double x = Y * v.Z - Z * v.Y;
            double y = Z * v.X - X * v.Z;
            double z = X * v.Y - Y * v.X;
            return new Vector(x, y, z);
        }

        public Vector Normalize()
        {
            return new Vector(X, Y, Z) / Length;
        }

        private double GetLength()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            if (v1 == null || v2 == null) throw new ArgumentNullException();
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1 == null || v2 == null) throw new ArgumentNullException();
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector operator *(Vector v, double scalar)
        {
            if (v == null) throw new ArgumentNullException();
            return new Vector(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        public static Vector operator /(Vector v, double scalar)
        {
            if (v == null) throw new ArgumentNullException();
            return new Vector(v.X / scalar, v.Y / scalar, v.Z / scalar);
        }

        public Vector3F ToStruct()
        {
            return new Vector3F(X,Y,Z);        
        }


    }
}
