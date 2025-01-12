using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using System.IO;

namespace ASCIIStl.Rendering
{
    public class Shader
    {
        public string VertexProgram { get; private set; }
        public string FragmentProgram { get; private set; }
        private int Handle { get; set; }

        public Shader(string vertexPath, string fragmentPath)
        {
            VertexProgram = File.ReadAllText(vertexPath);
            FragmentProgram = File.ReadAllText(fragmentPath);
        }

        public void Start()
        {
            try
            {
                Handle = GL.CreateProgram();

                int vertexShader = GL.CreateShader(ShaderType.VertexShader);
                GL.ShaderSource(vertexShader, VertexProgram);
                GL.CompileShader(vertexShader);
                CheckShaderCompileErrors(vertexShader, "VERTEX");

                int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                GL.ShaderSource(fragmentShader, FragmentProgram);
                GL.CompileShader(fragmentShader);
                CheckShaderCompileErrors(fragmentShader, "FRAGMENT");

                GL.AttachShader(Handle, vertexShader);
                GL.AttachShader(Handle, fragmentShader);
                GL.LinkProgram(Handle);
                CheckProgramLinkErrors(Handle);

                GL.DeleteShader(vertexShader);
                GL.DeleteShader(fragmentShader);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during Shader initialization: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                throw;
            }
        }

        private void CheckShaderCompileErrors(int shader, string type)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                Debug.WriteLine($"ERROR::SHADER_COMPILATION_ERROR of type: {type}\n{infoLog}\n -- --------------------------------------------------- -- ");
            }
        }

        private void CheckProgramLinkErrors(int program)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                Debug.WriteLine($"ERROR::PROGRAM_LINKING_ERROR\n{infoLog}\n -- --------------------------------------------------- -- ");
            }
        }

        public void Bind()
        {
            GL.UseProgram(Handle);
        }

        public void Delete()
        {
            GL.DeleteProgram(Handle);
        }

        public int GetUniform(string variable)
        {
            return GL.GetUniformLocation(Handle, variable);
        }
    }
}
