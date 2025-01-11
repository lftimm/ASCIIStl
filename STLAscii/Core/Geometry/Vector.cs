﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Core.Geometry
{
    public class Vector : XYZ
    {
        // Properties
        public float Length { get => GetLength(); }

        // Constructor
        public Vector(float x, float y, float z) : base(x, y, z) { }

        public Vector(double x, double y, double z) : base((float)x, (float)y, (float)z) { }

        // Public Methods
        public float DotProduct(Vector v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        public Vector CrossProduct(Vector v)
        {
            float x = Y * v.Z - Z * v.Y;
            float y = Z * v.X - X * v.Z;
            float z = X * v.Y - Y * v.X;
            return new Vector(x, y, z);
        }

        public Vector Normalize()
        {
            return new Vector(X, Y, Z) / Length;
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

        public static Vector operator *(Vector v, float scalar)
        {
            if (v == null) throw new ArgumentNullException();
            return new Vector(v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        public static Vector operator /(Vector v, float scalar)
        {
            if (v == null) throw new ArgumentNullException();
            return new Vector(v.X / scalar, v.Y / scalar, v.Z / scalar);
        }

        // Private Methods
        private float GetLength()
        {
            return MathF.Sqrt(X * X + Y * Y + Z * Z);
        }

    }
}
