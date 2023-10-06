using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Slugs
{
    public class Camera
    {
        public Matrix Mat { get; private set; }
        public Matrix ScaleMatrix { get; private set; }
        MouseState currentMouse;
        MouseState previousMouse;

        int xTrans = General.screenWidth / 2;
        int yTrans = General.screenHeight / 2;

        int xCenter;
        int yCenter;

        float lerp = 0.93f;

        float scale;
        IFocus focus;
        public Vector2 position;
        public Camera(IFocus focus)
        {
            this.focus = focus;
            position = Vector2.Zero;
            previousMouse = Mouse.GetState();
            scale = 2f;
        }

        public void ChangeCamFocus(IFocus focus)
        {
            this.focus = focus;
        }

        public void Update()
        {
            xCenter = (int)(xTrans / scale);
            yCenter = (int)(yTrans / scale);
            currentMouse = Mouse.GetState();

            if (currentMouse.ScrollWheelValue - previousMouse.ScrollWheelValue > 0)
            {
                scale += 0.1f;
                lerp = 1f;
            }
            else if (currentMouse.ScrollWheelValue - previousMouse.ScrollWheelValue < 0)
            {
                scale -= 0.1f;
                lerp = 1f;
            }
            else
                lerp = 0.93f;

            if (scale < 1)
                scale = 1;

            ScaleMatrix = Matrix.Lerp(ScaleMatrix, Matrix.CreateScale(scale), 0.09f);

            previousMouse = currentMouse;

            Mat = Matrix.CreateTranslation(-position.X, -position.Y, 0) * ScaleMatrix * 
                  Matrix.CreateTranslation(xTrans, yTrans, 0);

            position = Vector2.Lerp(focus.Position + Vector2.UnitY * 40f, position, lerp);

            if (position.X - xCenter < 0)
                position = new Vector2(xCenter, position.Y);
            if (position.Y - yCenter < 0)
                position = new Vector2(position.X, yCenter);

            if (position.X + xCenter > General.screenWidth)
                position = new Vector2(General.screenWidth - xCenter, position.Y);
            if (position.Y + yCenter > General.screenHeight)
                position = new Vector2(position.X, General.screenHeight - yCenter);
        }
    }
}
