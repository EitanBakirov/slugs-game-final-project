using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    public class Rocket : Shootable
    {
        public List<Vector2> SmokeList { get; set; }

        public Rocket(Texture2D texture, Vector2 position, Rectangle? sourceRectangle,
                        Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, Vector2 direction, float damage)
            : base(direction, damage, texture, position, sourceRectangle,
                        color, rotation, origin, scale, effects, layerDepth)
        {
            SmokeList = new List<Vector2>();
            Damage = damage;
            powerOfExplosion = 5;
        }

        public override void Update(Vector2 position, float angle)
        {
            if (TimeToActivate <= General.Time)
            {
                IsFlying = true;
                General.cam.ChangeCamFocus(this);
                Vector2 gravity = new Vector2(0, 1);
                Direction += gravity / 10.0f;
                Position += Direction;
                Angle = (float)Math.Atan2(Direction.X, -Direction.Y);

                for (int i = 0; i < 5; i++)
                {
                    Vector2 smokePos = Position;
                    smokePos.X += General.randomizer.Next(10) - 5;
                    smokePos.Y += General.randomizer.Next(10) - 5;
                    SmokeList.Add(smokePos);
                }
            }
            else
            {
                Position = position;
                Angle = angle;
                Vector2 up = new Vector2(0, -1);
                Matrix rotMatrix = Matrix.CreateRotationZ(Angle);
                Direction = Vector2.Transform(up, rotMatrix);
                Vector2 gravity = new Vector2(0, 1);
                Direction += gravity / 10.0f;
            }
        }

        public override void Draw()
        {
            if (IsFlying)
            {
                General.sb.Draw(
                        Texture, Position, SourceRectangle,
                        color, Angle, Origin,
                        Scale, Effects, 0);
                DrawSmoke();
            }
        }

        private void DrawSmoke()
        {
            foreach (Vector2 smokePos in SmokeList)
                General.sb.Draw(Dictionaries.TextureDictionary[FolderTextures.Graphics]["smoke"], smokePos, null, Color.White, 0, new Vector2(40, 35), 0.2f, SpriteEffects.None, 0);
        }

        public override void Reset()
        {
            SmokeList = new List<Vector2>();
        }

        public override void AddExplosion(Vector2 collisionPos, GameTime gameTime)
        {
            Explosion.AddExplosion(collisionPos, 10, 80.0f, 2000.0f, gameTime);
        }
    }
}
