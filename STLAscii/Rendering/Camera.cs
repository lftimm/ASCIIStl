using ASCIIStl.Core.Geometry;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Rendering
{
    public class Camera
    {
        private const float SPEED = 8f;
        private const float SENSITIVITY = 180f;

        private float Width { get; set; }
        private float Height { get; set; }

        public Vector Position { get; set; }
        public Vector Velocity { get; set; }

        public Vector BasisZ = Vector.BaseZ * -1;
        public Vector BasisX = Vector.BaseX;
        public Vector BasisY = Vector.BaseY;

        public Camera(float width, float height, Vector position) 
        {
            Width = width;
            Height = height;
            Position = position;
        }

        public Transform GetViewTransform() 
        {
            Vector target = Position + BasisZ;
            return Transform.FromMatrix4(Matrix4.LookAt(Position.X, Position.Y, Position.Z,
                                                        target.X, target.Y, target.Z,
                                                        BasisY.X, BasisY.Y, BasisZ.Z));
        }
        public Transform GetProjectionTransform() 
        {
            return Transform.FromMatrix4(Matrix4.CreatePerspectiveFieldOfView((float)(60 * Math.PI / 180), Width / Height, 0.1f, 100.0f));
        }
        public void UpdateVectors(KeyboardState input, MouseState mouse, FrameEventArgs e) 
        {
            InputController(input,mouse,e);
        }
        private void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            if(input.IsKeyDown(Keys.A))
            {
                Position -= BasisX* SPEED * (float)e.Time;
            } else if(input.IsKeyDown(Keys.D))
            {
                Position += BasisX * SPEED * (float)e.Time;
            } else if(input.IsKeyDown(Keys.W)) 
            {
                Position += BasisY * SPEED * (float)e.Time;
            } else if(input.IsKeyDown(Keys.S))
            {
                Position -= BasisY * SPEED * (float)e.Time;
            }

        }

    }
}
