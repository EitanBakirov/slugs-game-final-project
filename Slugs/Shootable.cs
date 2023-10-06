using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    public abstract class Shootable : Drawable
    {
        public float Damage { get; set; }
        public Vector2 Direction { get; set; }
        public bool IsFlying { get; set; }
        public float Angle { get; set; }
        public int TimeToActivate { get; set; }
        public int powerOfExplosion { get; set; }

        public Shootable(Vector2 direction, float damage, Texture2D texture, Vector2 position, Rectangle? sourceRectangle,
                        Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
            : base(texture, position, sourceRectangle,
                         color, rotation, origin, scale, effects, layerDepth)
        {
            Damage = damage;
            Direction = direction;
        }

        public Vector2 GetShootableNose()
        {
            Vector2 newOrigin = Vector2.Transform(new Vector2(0, -this.Origin.Y), Matrix.CreateRotationZ(this.Angle));
            newOrigin *= this.Scale;

            Vector2 pos = this.Position;
            pos += newOrigin;

            return pos;
        }

        public abstract void AddExplosion(Vector2 collisionPos, GameTime gameTime);

        public bool IsOutOfScreen()
        {
            bool isOutOfScreen = Position.Y > General.screenHeight;
            isOutOfScreen |= Position.X < 0;
            isOutOfScreen |= Position.X > General.screenWidth;

            return isOutOfScreen;
        }

        public abstract void Update(Vector2 position, float angle);

        public abstract void Reset();
    }
}
