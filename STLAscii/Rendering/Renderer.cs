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

        private int VertexArray { get; set; }
        private int VertexBuffer { get; set; }

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
            Vertices = face.ToArrayF();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            try
            {
                Debug.WriteLine("OnLoad: Initializing VAO and VBO");

                VertexArray = GL.GenVertexArray();
                VertexBuffer = GL.GenBuffer();

                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer);

                // Aqui que entram meus vértices
                GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);


                GL.BindVertexArray(VertexArray);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.BindVertexArray(0);

                Debug.WriteLine("OnLoad: Initializing Shader Program");

                ShaderProgram.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during OnLoad: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                Close();
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            try
            {
                Debug.WriteLine("OnUnload: Deleting VAO and VBO");
                GL.DeleteVertexArray(VertexArray);
                GL.DeleteBuffer(VertexBuffer);
                if (ShaderProgram != null)
                {
                    GL.DeleteProgram(ShaderProgram.Handle);
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
                GL.Clear(ClearBufferMask.ColorBufferBit);

                if (ShaderProgram != null)
                {
                    GL.UseProgram(ShaderProgram.Handle);
                    GL.BindVertexArray(VertexArray);

                    Transform model = Transform.Identity;
                    Transform view = Transform.Identity;

                    Transform translation = Transform.CreateTranslation(0f, 0f, -3f);

                    model = Transform.CreateRotationAtY(45);
                    model *= translation;

                    Transform projection = Transform.FromMatrix4(Matrix4.CreatePerspectiveFieldOfView((float)(20 * Math.PI / 180), Width / Height, 0.1f, 100.0f));


                    int transformLocation = GL.GetUniformLocation(ShaderProgram.Handle, "transform");
                    

                    float[] transform = (model*view*projection).Values;
                    GL.UniformMatrix4(transformLocation, 1, true, transform);


                    GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
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
