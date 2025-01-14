using ASCIIStl.Core.Geometry;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ASCIIStl.Core
{
    public class STLObject
    {
        private string FileAsString { get; set; }
        public List<Face> Faces { get; private set; }
        public List<Vector> Vertices { get; private set; }
        public  List<Vector>  UniqueVertices { get; private set; }
        public uint[] ElementIndexes { get; private set; }

        public STLObject(string path)
        {
            FileAsString = File.ReadAllText(path);
            Faces = ParseFile();
            Vertices = Faces.SelectMany(x => x.Vertices).ToList();

            UniqueVertices = Vertices.Distinct().ToList();
            ElementIndexes = OrderVertices();
        }


        // Refatorar isto
        private uint[] OrderVertices()
        {
            // Map each unique vector to an index
            var vectorIndexMap = new Dictionary<Vector, uint>();
            var uniqueVectors = new List<Vector>();
            var elementBuffer = new List<uint>();

            uint currentIndex = 0;
            foreach (var vector in UniqueVertices)
            {
                if (!vectorIndexMap.ContainsKey(vector))
                {
                    vectorIndexMap[vector] = currentIndex++;
                    uniqueVectors.Add(vector);
                }
            }


            // Iterate through vertices in groups of three
            for (int i = 0; i < Vertices.Count; i += 3)
            {
                if (i + 2 >= Vertices.Count)
                    throw new Exception("Vertices array does not contain a complete set of triangles.");

                Vector currentFaceNormal = Faces.Select(x => x.FaceNormal).ToList()[i/3];

                // Extract the triangle (3 vertices)
                var triangle = new List<Vector>
                                {
                                    Vertices[i],
                                    Vertices[i + 1],
                                    Vertices[i + 2]
                                };

                // Ensure clockwise order for OpenGL
                if (!IsClockwise(triangle[0], triangle[1], triangle[2],currentFaceNormal))
                {
                    // Swap the last two vertices to make it clockwise
                    (triangle[1], triangle[2]) = (triangle[2], triangle[1]);
                }

                // Add triangle indices to the element buffer
                // Add triangle indices to the element buffer
                for (int j = 0; j < triangle.Count; j++)
                {
                    elementBuffer.Add(vectorIndexMap[triangle[j]]);
                }
                
            }

            // (Optional) You could return the unique vector list too if needed
            // UniqueVectors = uniqueVectors;

            return elementBuffer.ToArray();
        }

        // Helper function to check if a triangle is clockwise
        private bool IsClockwise(Vector v0, Vector v1, Vector v2, Vector normal)
        {
            // Calculate cross product of two triangle edges
            Vector edge1 = v1 - v0;
            Vector edge2 = v2 - v0;
            Vector crossProduct = edge1.CrossProduct(edge2);
            if(normal != crossProduct)
                crossProduct = normal;

            // Check the Z component of the cross product (assumes viewing along Z-axis)
            return crossProduct.Z < 0;
        }


        private List<Face> ParseFile()
        {
            var faces = new List<Face>();
            var lines = FileAsString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().StartsWith("facet normal"))
                {
                    var vertices = new List<Vector>();
                    i += 2;

                    for (int j = 0; j < 3; j++)
                    {
                        var vertexLine = lines[i + j].Trim();
                        if (vertexLine.StartsWith("vertex"))
                        {
                            var parts = vertexLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                            float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                            float z = float.Parse(parts[3], CultureInfo.InvariantCulture);
                            vertices.Add(new Vector(x, y, z).Normalize());
                        }
                    }

                    i += 4;
                    faces.Add(new Face(vertices));
                }
            }

            return faces;
        }
        private void PrintOrderArray(uint[] order)
        {
            for (int i = 0; i < order.Length; i++)
            {
                Debug.Write(order[i] + " ");
                if ((i + 1) % 3 == 0)
                {
                    Debug.WriteLine("");
                }
            }
            if (order.Length % 3 != 0)
            {
                Debug.WriteLine("");
            }
        }
    }
}
