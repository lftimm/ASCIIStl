using System;
using System.Numerics;
using System.Windows.Markup;

namespace ASCIIStl.Core.Geometry
{
    public partial class Transform
    {
        // PROPERTIES
        public static Transform Identity = new Transform( new float[] {
            1f,0,0, 0,
            0,1f,0,0,
            0,0,1f,0,
            0,0,0,1f,
        });

        public float[] Values { get; private set; }

        // CONSTRUCTOR
        private Transform(ICollection<float> values)
        {
            if (values.Count < 16)
                throw new ArgumentException($"Invalid input, must have 16 numbers, was given {values.Count}");

            Values = values.ToArray();
        }

        private Transform(ICollection<double> values)
        {
            if (values.Count < 16)
                throw new ArgumentException($"Invalid input, must have 16 numbers, was given {values.Count}");

            if (values.Any(x => x < float.MaxValue))
                throw new ArgumentException($"Cast made impossible, a number is above the float maximum");

            Values = values.Select(x=>(float)x).ToArray();
        }

    }
}
