using ASCIIStl.Core;
using ASCIIStl.Core.Geometry;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Diagnostics;
using System.Numerics;

namespace ASCIIStl.Rendering
{
    public sealed class Renderer : GameWindow
    {
        private static Renderer _instance;

        private int Width { get; set; }
        private int Height { get; set; }
        private Shader ShaderProgram { get; set; }

        float[] Vertices;
        uint[] IndexVertices;

        private VertexArray  VAO { get; set; }
        private VertexBuffer VBO { get; set; }
        int ElementBuffer { get; set; }
        double rotate = 45;

        public static Renderer GetRender(int width, int height, string title, Shader shaderProgram, Face face)
        {
            if (_instance == null)
            {
                _instance = new Renderer(width, height, title, shaderProgram, face);
            }
            return _instance;
        }

        private Renderer(int width, int height, string title, Shader shaderProgram, Face face)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title })
        {
            Width = width;
            Height = height;

            ShaderProgram = shaderProgram;
            //Vertices = face.ToArrayF();
            STLObject myObject = new("C:\\Users\\lftim\\Documents\\Projects\\STLAscii\\STLAscii\\STLDemos\\cubeTest.stl");

            //Vertices = [-0.5f, -0.5f, 0,
            //            -0.5f,  0.5f, 0,
            //             0.5f, -0.5f, 0,
            //             0.5f,  0.5f, 0];

            //IndexVertices = [0,2,1,
            //                 0,2,3];

            Vertices = myObject.UniqueVertices.SelectMany(x => x.ToFloatArray()).ToArray();
            IndexVertices = myObject.ElementIndexes;
            //foreach (var item in IndexVertices)
            //{
            //    Debug.Write($"{item}");
            //}
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            try
            {
                Debug.WriteLine("OnLoad: Initializing VAO and VBO");

                VAO = new VertexArray();
                VBO = new VertexBuffer(Vertices);
                VAO.LinkToVAO(0,3, VBO);

                ElementBuffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBuffer);
                GL.BufferData(BufferTarget.ElementArrayBuffer, IndexVertices.Length*sizeof(uint), IndexVertices, BufferUsageHint.StaticDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                

                ShaderProgram.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during OnLoad: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                Close();
            }

            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            try
            {
                Debug.WriteLine("OnUnload: Deleting VAO and VBO");
                VAO.Delete();
                VBO.Delete();
                GL.DeleteBuffer(ElementBuffer);
                if (ShaderProgram != null)
                {
                    ShaderProgram.Delete();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during OnUnload: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            try
            {
                GL.ClearColor(0f, 0f, 0f, 1f);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                if (ShaderProgram != null)
                {
                    ShaderProgram.Bind();
                    VAO.Bind();
                    VBO.Bind();
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBuffer);
                    
                        
                    Transform model = Transform.Identity;
                    Transform view = Transform.Identity;

                    Transform translation = Transform.CreateTranslation(0f, 0f, -3f);

                    model = Transform.CreateRotationAtY((float)rotate);
                    model *= translation;
                    rotate += 1e-3;
                    Transform projection = Transform.FromMatrix4(Matrix4.CreatePerspectiveFieldOfView((float)(60 * Math.PI / 180), Width / Height, 0.1f, 100.0f));

                    int transformLocation = ShaderProgram.GetUniform("transform");
                    

                    float[] transform = (model * view * projection).Values;
                    GL.UniformMatrix4(transformLocation, 1, true, transform);

                    GL.DrawElements(PrimitiveType.Triangles, IndexVertices.Length, DrawElementsType.UnsignedInt, 0);
                    //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                }

                Context.SwapBuffers();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during OnRenderFrame: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                Close();
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            try
            {
                GL.Viewport(0, 0, e.Width, e.Height);
                Width = e.Width;
                Height = e.Height;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during OnResize: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
