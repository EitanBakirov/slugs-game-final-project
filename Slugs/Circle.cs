using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    public class Circle
    {
        public Vector2 Center;
        public float Radius;
        public Circle(Vector2 Center, float Radius)
        {
            this.Center = Center;
            this.Radius = Radius;
        }
    }
}
