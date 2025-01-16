using ASCIIStl.Core.Geometry;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace ASCIIStl.Rendering
{
    public class Camera
    {
        private const float SPEED = 5f;
        private const float SENSITIVITY = 180f;

        private float Width { get; set; }
        private float Height { get; set; }

        public Vector Position { get; set; }
        public Vector FirstPosition { get; set; }
        

        public Vector BasisZ = Vector.BaseZ * -1;
        public Vector BasisX = Vector.BaseX;
        public Vector BasisY = Vector.BaseY;

        public Vector lastPos { get; set; }

        private float Yaw = -90f;
        private float Pitch;

        public Camera(float width, float height, Vector position) 
        {
            Width = width;
            Height = height;
            Position = position;
            FirstPosition = position;
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
            return Transform.FromMatrix4(Matrix4.CreatePerspectiveFieldOfView((float)(90 * Math.PI / 180), Width / Height, 0.1f, 100.0f));
        }
        public void Update(KeyboardState input, MouseState mouse, FrameEventArgs e) 
        {
            InputController(input,mouse,e);
        }
        private void InputController(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                Vector2 deltaMovement = mouse.Position - mouse.PreviousPosition;
                Position += BasisX * deltaMovement.X * SPEED * (float)e.Time; 
                Position += BasisY * deltaMovement.Y * SPEED * (float)e.Time; 
            }
            else if (mouse.IsButtonDown(MouseButton.Right))
            {
                Vector2 deltaMovement = mouse.Position - mouse.PreviousPosition;
                Yaw += deltaMovement.X * SENSITIVITY * (float)e.Time;
                Pitch += deltaMovement.Y * SENSITIVITY * (float)e.Time;

                UpdateVectors();
            } else if (mouse.ScrollDelta.Length > 0)
            {
                Position -= new Vector(0, 0, mouse.ScrollDelta.Y);
            }





        }

        public void UpdateVectors()
        {
            float yawRad = Yaw * float.Pi / 180;
            float pitchRad = Pitch * float.Pi / 180;

            BasisZ = new Vector(MathF.Cos(pitchRad) * MathF.Cos(yawRad),
                                 MathF.Sin(pitchRad),
                                 MathF.Cos(pitchRad) * MathF.Sin(yawRad)).Normalize();
            BasisX = BasisZ.CrossProduct(Vector.BaseY).Normalize();
            BasisY = BasisX.CrossProduct(BasisZ).Normalize();


        }
    }
}
