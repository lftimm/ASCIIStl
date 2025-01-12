using ASCIIStl.Core.Geometry;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Rendering
{
    public class VertexBuffer
    {
        private int Handle {  get; set; }
        public VertexBuffer(float[] data)
        {
            Handle = GL.GenBuffer();

            // Convert List<Vector> to List<float>
           

            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data.ToArray(), BufferUsageHint.StaticDraw);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
        }
        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void Delete()
        {
            GL.DeleteBuffer(Handle);
        }
    }
}
