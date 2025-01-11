using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenTK.Graphics.OpenGL.GL;

namespace ASCIIStl.Core.Geometry
{
    public partial class Transform
    {
        public static Transform CreateRotationAtX(float angle, bool convertToRadians = false)
        {
            if (convertToRadians)
                angle = (float)(angle * Math.PI / 180);


            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            float[] matrix = new float[16]{
                1, 0, 0, 0,
                0, cos, -sin, 0,
                0, sin, cos, 0,
                0, 0, 0, 1
            };

            return new Transform(matrix);
        }

        public static Transform CreateRotationAtY(float angle, bool convertToRadians = false)
        {
            if (convertToRadians)
                angle = (float)(angle * Math.PI / 180);

            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            float[] matrix = new float[16]{
                cos, 0, sin, 0,
                0, 1, 0, 0,
                -sin, 0, cos, 0,
                0, 0, 0, 1
            };

            return new Transform(matrix);
        }

        public static Transform CreateRotationAtZ(float angle, bool convertToRadians = false)
        {
            if (convertToRadians)
                angle = (float)(angle * Math.PI / 180);

            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            float[] matrix = new float[16]{
                cos, -sin, 0, 0,
                sin, cos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            };

            return new Transform(matrix);
        }
        public static Transform CreateTranslation(float x, float y, float z)
        {
            float[] matrix = new float[16]
            {
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                x, y, z, 1
            };

            return new Transform(matrix);
        }

        public static Transform CreateScaling(float scaleX, float scaleY, float scaleZ)
        {
            float[] matrix = new float[16]
            {
                scaleX, 0, 0, 0,
                0, scaleY, 0, 0,
                0, 0, scaleZ, 0,
                0, 0, 0, 1
            };

            return new Transform(matrix);
        }

        public static Transform CreateRotation(float angleX, float angleY, float angleZ, bool convertToRadians = false)
        {
            if (convertToRadians)
            {
                angleX = (float)(angleX * Math.PI / 180);
                angleY = (float)(angleY * Math.PI / 180);
                angleZ = (float)(angleZ * Math.PI / 180);
            }

            Transform rotationX = CreateRotationAtX(angleX);
            Transform rotationY = CreateRotationAtY(angleY);
            Transform rotationZ = CreateRotationAtZ(angleZ);

            return rotationZ * rotationY * rotationX; // Order of rotations matters
        }

        public static Transform FromMatrix4(Matrix4 matrix)
        {
            float[] values = new float[16]
            {
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
            };

            return new Transform(values);
        }

        // Operators
        public static Transform operator *(Transform a, Transform b)
        {
            float[] aValues = a.Values;
            float[] bValues = b.Values;
            float[] matrix = new float[16]
            {
                aValues[0] * bValues[0] + aValues[1] * bValues[4] + aValues[2] * bValues[8] + aValues[3] * bValues[12],
                aValues[0] * bValues[1] + aValues[1] * bValues[5] + aValues[2] * bValues[9] + aValues[3] * bValues[13],
                aValues[0] * bValues[2] + aValues[1] * bValues[6] + aValues[2] * bValues[10] + aValues[3] * bValues[14],
                aValues[0] * bValues[3] + aValues[1] * bValues[7] + aValues[2] * bValues[11] + aValues[3] * bValues[15],

                aValues[4] * bValues[0] + aValues[5] * bValues[4] + aValues[6] * bValues[8] + aValues[7] * bValues[12],
                aValues[4] * bValues[1] + aValues[5] * bValues[5] + aValues[6] * bValues[9] + aValues[7] * bValues[13],
                aValues[4] * bValues[2] + aValues[5] * bValues[6] + aValues[6] * bValues[10] + aValues[7] * bValues[14],
                aValues[4] * bValues[3] + aValues[5] * bValues[7] + aValues[6] * bValues[11] + aValues[7] * bValues[15],

                aValues[8] * bValues[0] + aValues[9] * bValues[4] + aValues[10] * bValues[8] + aValues[11] * bValues[12],
                aValues[8] * bValues[1] + aValues[9] * bValues[5] + aValues[10] * bValues[9] + aValues[11] * bValues[13],
                aValues[8] * bValues[2] + aValues[9] * bValues[6] + aValues[10] * bValues[10] + aValues[11] * bValues[14],
                aValues[8] * bValues[3] + aValues[9] * bValues[7] + aValues[10] * bValues[11] + aValues[11] * bValues[15],

                aValues[12] * bValues[0] + aValues[13] * bValues[4] + aValues[14] * bValues[8] + aValues[15] * bValues[12],
                aValues[12] * bValues[1] + aValues[13] * bValues[5] + aValues[14] * bValues[9] + aValues[15] * bValues[13],
                aValues[12] * bValues[2] + aValues[13] * bValues[6] + aValues[14] * bValues[10] + aValues[15] * bValues[14],
                aValues[12] * bValues[3] + aValues[13] * bValues[7] + aValues[14] * bValues[11] + aValues[15] * bValues[15]
            };

            return new Transform(matrix);
        }

        public static Transform operator +(Transform A, Transform B)
        {
            float[] a = A.Values;
            float[] b = B.Values;
            float[] matrix = new float[16] 
            {
                a[0]+b[0] , a[1]+b[1] , a[2]+b[2] , a[3]+b[3] ,
                a[4]+b[4] , a[5]+b[5] , a[6]+b[6] , a[7]+b[7] ,
                a[8]+b[8] , a[9]+b[9] , a[10]+b[10] , a[11]+b[11] ,
                a[12]+b[12], a[13]+b[13], a[14]+b[14], a[15]+b[15]
            };

            return new Transform(matrix);
        }


        public static Transform operator -(Transform A, Transform B)
        {
            float[] a = A.Values;
            float[] b = B.Values;
            float[] matrix = new float[16]
            {
                a[0] - b[0], a[1] - b[1], a[2] - b[2], a[3] - b[3],
                a[4] - b[4], a[5] - b[5], a[6] - b[6], a[7] - b[7],
                a[8] - b[8], a[9] - b[9], a[10] - b[10], a[11] - b[11],
                a[12] - b[12], a[13] - b[13], a[14] - b[14], a[15] - b[15]
            };

            return new Transform(matrix);
        }

        public static Transform operator /(Transform a, float scalar)
        {
            float[] aValues = a.Values;
            float[] matrix = new float[16]
            {
                aValues[0] / scalar, aValues[1] / scalar, aValues[2] / scalar, aValues[3] / scalar,
                aValues[4] / scalar, aValues[5] / scalar, aValues[6] / scalar, aValues[7] / scalar,
                aValues[8] / scalar, aValues[9] / scalar, aValues[10] / scalar, aValues[11] / scalar,
                aValues[12] / scalar, aValues[13] / scalar, aValues[14] / scalar, aValues[15] / scalar
            };

            return new Transform(matrix);
        }

        public static Transform operator *(Transform a, float scalar)
        {
            float[] aValues = a.Values;
            float[] matrix = new float[16]
            {
                aValues[0] * scalar, aValues[1] * scalar, aValues[2] * scalar, aValues[3] * scalar,
                aValues[4] * scalar, aValues[5] * scalar, aValues[6] * scalar, aValues[7] * scalar,
                aValues[8] * scalar, aValues[9] * scalar, aValues[10] * scalar, aValues[11] * scalar,
                aValues[12] * scalar, aValues[13] * scalar, aValues[14] * scalar, aValues[15] * scalar
            };

            return new Transform(matrix);
        }

    }
}
