using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Rendering
{
    public class VertexArray
    {
        private int Handle { get; set; }

        public VertexArray()
        {
            Handle = GL.GenVertexArray();
            GL.BindVertexArray(Handle);
        }

        public void LinkToVAO(int location, int size, VertexBuffer buffer)
        {
            Bind();
            buffer.Bind();
            GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(location);
            Unbind();
        }
        public void Bind()
        {
            GL.BindVertexArray(Handle);
        }
        public void Unbind()
        {
            GL.BindVertexArray(0);
        }
        public void Delete()
        {
            GL.DeleteVertexArray(Handle);
        }
    }
}
