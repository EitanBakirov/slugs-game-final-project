using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    public class Bullet : Shootable
    {

        public Bullet(Texture2D texture, Vector2 position, Rectangle? sourceRectangle,
                        Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, Vector2 direction, float damage)
            : base(direction, damage, texture, position, sourceRectangle,
                        color, rotation, origin, scale, effects, layerDepth)
        {
            Damage = damage;
            powerOfExplosion = 1;
        }

        public override void Update(Vector2 position, float angle)
        {
            if (TimeToActivate <= General.Time)
            {
                IsFlying = true;
                Position += Direction;
                Angle = (float)Math.Atan2(Direction.X, -Direction.Y);
            }
            else
            {
                Position = position;
                Angle = angle;
                Vector2 up = new Vector2(0, -1);
                Matrix rotMatrix = Matrix.CreateRotationZ(Angle);
                Direction = Vector2.Transform(up, rotMatrix);
                Direction *= 8f;
            }
        }


        public override void Draw()
        {
            if (IsFlying)
                General.sb.Draw(
                        Texture, Position, SourceRectangle,
                        color, Angle, Origin,
                        Scale, Effects, 0);
        }

        public override void Reset()
        {
        }

        public override void AddExplosion(Vector2 collisionPos, GameTime gameTime)
        {
            Explosion.AddExplosion(collisionPos, 1, 20.0f, 10.0f, gameTime);
        }
    }
}
