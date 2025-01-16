using ASCIIStl.Core;
using ASCIIStl.Core.Geometry;
using ASCIIStl.Rendering;
using OpenTK.Graphics.ES11;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Vector = ASCIIStl.Core.Geometry.Vector;

namespace ASCIIStl
{
    public class Program
    {
        private const int HEIGHT = 1280;
        private const int WIDTH = 720;
        private const string TITLE = "Example Program";
        private const string VERTEX_PATH = "C:\\Users\\lftim\\Documents\\Projects\\STLAscii\\STLAscii\\Rendering\\Shaders\\shader.vert";
        private const string FRAGMENT_PATH = "C:\\Users\\lftim\\Documents\\Projects\\STLAscii\\STLAscii\\Rendering\\Shaders\\shader.frag";
        private const string STL1_PATH = "C:\\Users\\lftim\\Documents\\Projects\\STLAscii\\STLAscii\\STLDemos\\test.stl";
        private const string STL2_PATH = "C:\\Users\\lftim\\Documents\\Projects\\STLAscii\\STLAscii\\STLDemos\\Sphericon.stl";
        private const string STL3_PATH = "C:\\Users\\lftim\\Documents\\Projects\\STLAscii\\STLAscii\\STLDemos\\cube.stl";
        private const string STL4_PATH = "C:\\Users\\lftim\\Documents\\Projects\\STLAscii\\STLAscii\\STLDemos\\square.stl";
        public static void Main(string[] args)
        {
            Vector v1 = new Vector(0, 0.5, 0);
            Vector v2 = new Vector(-0.5, -0.5, 0);
            Vector v3 = new Vector(0.5f, -0.5, 0);
            Face face = new Face([v1, v2, v3]);


            Shader shaderProgram = new(VERTEX_PATH, FRAGMENT_PATH);
            using (var render = Renderer.GetRender(HEIGHT, WIDTH, TITLE, shaderProgram, face))
            {
                render.Run();
            }

            //STLObject sTLObject = new(STL4_PATH);
            //foreach (var a in sTLObject.VertForEBO)
            //{
            //    Debug.WriteLine(a);
            //}
        }
    }
}

