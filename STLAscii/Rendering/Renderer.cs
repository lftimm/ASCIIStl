using ASCIIStl.Core;
using ASCIIStl.Core.Geometry;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Diagnostics;

namespace ASCIIStl.Rendering
{
    public sealed class Renderer : GameWindow
    {
        private static Renderer? _instance;

        private int Width { get; set; }
        private int Height { get; set; }
        private string BaseTitle { get; set; }
        private Shader ShaderProgram { get; set; }
        private Camera camera { get; set; }

        float[] Vertices;
        uint[] IndexVertices;

        private VertexArray?  VAO { get; set; }
        private VertexBuffer? VBO { get; set; }
        private ElementBuffer? EBO { get; set; }

        double rotate = 45;
        private double frameTime;

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
            Height = height;
            Width = width;
            ShaderProgram = shaderProgram;
            BaseTitle = title;
            //Vertices = face.ToArrayF();
            STLObject myObject = new("C:\\Users\\lftim\\Documents\\Projects\\STLAscii\\STLAscii\\STLDemos\\cubeTest.stl");

            //Vertices = [-0.5f, -0.5f, 0,
            //       
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
                EBO = new ElementBuffer(IndexVertices);
                VAO.LinkToVAO(0,3, VBO);

                ShaderProgram.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during OnLoad: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                Close();
            }

            camera = new Camera(Width, Height, Vector.Zero);
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
                    EBO.Bind();

                    Transform model = Transform.Identity;
                    Transform view = camera.GetViewTransform();
                    Transform projection = camera.GetProjectionTransform();


                    Transform translation = Transform.CreateTranslation(0f, 0f, -3f);

                    model = Transform.CreateRotationAtY((float)rotate);
                    model *= translation;
                    rotate += 1e-3;

                    int transformLocation = ShaderProgram.GetUniform("transform");


                    float[] transform = (model * view * projection).Values;
                    GL.UniformMatrix4(transformLocation, 1, true, transform);

                    GL.DrawElements(PrimitiveType.Triangles, IndexVertices.Length, DrawElementsType.UnsignedInt, 0);
                    //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                }
                UpdateTitleWithFPS(args);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during OnRenderFrame: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                Close();
            }
        }

        private void UpdateTitleWithFPS(FrameEventArgs args)
        {
            frameTime += args.Time;
            if (frameTime >= 0.25)
            {
                Title = $"{BaseTitle} {0.25 / args.Time:F2}";
                frameTime = 0.0;
            }
            Context.SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            KeyboardState input = KeyboardState;
            MouseState mouse = MouseState;
            base.OnUpdateFrame(args);
            camera.UpdateVectors(input, mouse, args);
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
