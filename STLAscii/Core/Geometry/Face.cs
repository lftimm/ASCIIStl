using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Core.Geometry
{
    public class Face
    {
        public Vector V1 { get; private set; }
        public Vector V2 { get; private set; }
        public Vector V3 { get; private set; }
        public List<Vector> Vertices { get => [V1, V2, V3]; }
        public Vector FaceNormal { get; private set; }

        public Face(ICollection<Vector> vertexList)
        {
            if (vertexList == null)
                throw new Exception("Null collection of verticies passed");
            if (vertexList.Count != 3)
                throw new Exception("Three vertices must be given");
            if (!IsValid(vertexList))
                throw new Exception("Vertices must be different");

            V1 = vertexList.First();
            V2 = vertexList.ElementAt(1);
            V3 = vertexList.Last();

            FaceNormal = GetFaceNormal();
        }

        public Face(Vector v1, Vector v2, Vector v3)
        {
            if ((new Vector[3] { v1, v2, v3 }).Any(x => x == null))
                throw new Exception("Null vector passed");

            V1 = v1;
            V2 = v2;
            V3 = v3;
 
            FaceNormal = GetFaceNormal();
        }

        // Public Methods
        public override string ToString()
        {
            return $"V1: {V1} " +
                $"V2: {V2} " +
                $"V3: {V3} " +
                $"FaceNormal: {FaceNormal}";
        }

        // Private Methods
        private bool IsValid(ICollection<Vector> vertexList)
        {
            var result = new HashSet<Vector>(vertexList);
            return result.Count == vertexList.Count;
        }

        private Vector GetFaceNormal()
        {
            Vector v1 = V1.ToVector();
            Vector v2 = V2.ToVector();
            Vector v3 = V3.ToVector();

            Vector vv1 = v2 - v1;
            Vector vv2 = v3 - v1;


            Vector result = vv1.CrossProduct(vv2);
            return result.Normalize();
        }

        public float[] ToArrayF()
        {
            Vector v1 = V1;
            Vector v2 = V2;
            Vector v3 = V3;

            return new float[]
            {
                v1.X,v1.Y,v1.Z,
                v2.X,v2.Y,v2.Z,
                v3.X,v3.Y,v3.Z
            };
        }
    }
}
