using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASCIIStl.Core.Geometry;

namespace ASCIIStl.Core
{
    public class STLObject
    {
        public List<Face> Faces { get; set; }
        private STLFile FileSTL { get; set; }
        private string FileAsString { get => FileSTL.ThisFile; }
        public STLObject(string path)
        {
            FileSTL = new STLFile(path);
            Faces = ParseFile();
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
                            double x = double.Parse(parts[1], CultureInfo.InvariantCulture);
                            double y = double.Parse(parts[2], CultureInfo.InvariantCulture);
                            double z = double.Parse(parts[3], CultureInfo.InvariantCulture);
                            vertices.Add(new Vector(x, y, z));
                        }
                    }

                    i += 4; 
                    faces.Add(new Face(vertices));
                }
            }

            return faces;
        }
    }
}
