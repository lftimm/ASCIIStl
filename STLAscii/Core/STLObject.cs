using ASCIIStl.Core.Geometry;
using System.Diagnostics;
using System.Globalization;

namespace ASCIIStl.Core
{
    public class STLObject
    {
        private string FileAsString { get; set; }
        private List<Face> Faces { get; set; }
        public List<Vector> VertsAsVectors { get; private set; }
        public float[] Vertices { get; private set; }
        public uint[] VertForEBO { get; private set; }

        public STLObject(string path)
        {
            FileAsString = File.ReadAllText(path);
            Faces = ParseFile();
            VertsAsVectors = Faces.SelectMany(x => x.Vertices).ToList();
            Vertices = Faces.SelectMany(x => x.ToArrayF()).ToArray();
            VertForEBO = OrderVertices();
        }

        private uint[] OrderVertices()
        {

  
            return order.ToArray();
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
                            vertices.Add(new Vector(x, y, z));
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
