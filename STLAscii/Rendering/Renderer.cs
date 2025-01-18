using ASCIIStl.Core;
using ASCIIStl.Core.Geometry;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ASCIIStl.Rendering
{
    public sealed class Renderer : GameWindow
    {
        private static Renderer? _instance;
        private ImGuiController _controller;

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

        public static Renderer GetRender(int width, int height, string title, Shader shaderProgram, STLObject myObject)
        {
            if (_instance == null)
            {
                _instance = new Renderer(width, height, title, shaderProgram, myObject);
            }
            return _instance;
        }

        private Renderer(int width, int height, string title, Shader shaderProgram, STLObject myObject)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = new Vector2i(width, height), Title = title })
        {            
            Height = height;
            Width = width;
            ShaderProgram = shaderProgram;
            BaseTitle = title;

            Vertices = myObject.UniqueVertices.SelectMany(x => x.ToFloatArray()).ToArray();
            IndexVertices = myObject.ElementIndexes;
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

                _controller = new ImGuiController(50, 50);
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
            GL.Enable(EnableCap.ScissorTest);
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
                GL.Scissor(Width / 4, 0, Width, Height);
                _controller.Update(this, (float)args.Time);
                GL.ClearColor(1, 1, 1, 0);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

                if (ShaderProgram != null)
                {
                    ShaderProgram.Bind();
                    VAO.Bind();
                    VBO.Bind();
                    EBO.Bind();

                    ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, 0));
                    ImGui.SetNextWindowSize(new System.Numerics.Vector2(Width / 4, Height));
                    ImGui.Begin("Configurações", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse);
                    ImGui.SetWindowFontScale(1.5f);
                    ImGui.End();
                    _controller.Render();
                    ImGuiController.CheckGLError("End of frame");

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
            camera.Update(input, mouse, args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            try
            {
                GL.Viewport(0, 0, e.Width, e.Height);
                Width = e.Width;
                Height = e.Height;
                _controller.WindowResized(e.Width, e.Height);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during OnResize: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
