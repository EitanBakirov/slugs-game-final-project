using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    static class Explosion
    {
        public static Texture2D Texture;
        public static Color[,] explosionColorArray;
        public static List<Particle> particleList;

        public static void Create(Texture2D texture)
        {
            Texture = texture;
            explosionColorArray = General.TextureTo2DArray(texture);
            particleList = new List<Particle>();
        }

        public static void UpdateParticles(GameTime gameTime)
        {
            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
            for (int i = particleList.Count - 1; i >= 0; i--)
            {
                Particle particle = particleList[i];
                float timeAlive = now - particle.BirthTime;

                if (timeAlive > particle.MaxAge)
                {
                    particleList.RemoveAt(i);
                }
                else
                {
                    // realtive age
                    float relAge = timeAlive / particle.MaxAge;

                    // the position according to a physics formula
                    particle.Position = 0.5f * particle.Accelaration * relAge * relAge + particle.Direction * relAge + particle.OrginalPosition;

                    // inverted age
                    float invAge = 1.0f - relAge;

                    // colors the particle according to the age of it - in te end it is gone (0)
                    particle.color = new Color(new Vector4(invAge, invAge, invAge, invAge));

                    Vector2 positionFromCenter = particle.Position - particle.OrginalPosition;
                    float distance = positionFromCenter.Length();
                    particle.Scale = (50.0f + distance) / 200.0f;

                    particleList[i] = particle;
                }
            }
        }

        public static void AddExplosion(Vector2 explosionPos, int numberOfParticles, float size, float maxAge, GameTime gameTime)
        {
            for (int i = 0; i < numberOfParticles; i++)
                AddExplosionParticle(explosionPos, size, maxAge, gameTime);

            // Add a crater on the foreground by randomizing the rotation of the texture and getting the matrix of it by the rotation, scale and position of the explosion.
            float rotation = (float)General.randomizer.Next(10);
            Matrix mat = Matrix.CreateTranslation(-Texture.Width / 2, -Texture.Height / 2, 0) * Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(size / (float)Texture.Width * 2.0f) * Matrix.CreateTranslation(explosionPos.X, explosionPos.Y, 0);
            Foreground.AddCrater(explosionColorArray, mat);
        }

        public static void AddExplosionParticle(Vector2 explosionPos, float explosionSize, float maxAge, GameTime gameTime)
        {

            float BirthTime = (float)gameTime.TotalGameTime.TotalMilliseconds;

            float particleDistance = (float)General.randomizer.NextDouble() * explosionSize;
            Vector2 displacement = new Vector2(particleDistance, 0);
            float angle = MathHelper.ToRadians(General.randomizer.Next(360));
            displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(angle));

            Vector2 direction = displacement * 2.0f;
            Vector2 accelaration = -direction;

            Particle particle = new Particle(BirthTime, maxAge, explosionPos, accelaration, direction, Texture, explosionPos, null, Color.White, 0, new Vector2(256, 256), 0.25f, SpriteEffects.None, 1);

            particleList.Add(particle);
        }

        public static void Draw()
        {

            for (int i = 0; i < particleList.Count; i++)
            {
                Particle particle = particleList[i];
                General.sb.Draw(Texture, particle.Position, particle.SourceRectangle, particle.color, i, particle.Origin, particle.Scale, particle.Effects, 0);
            }

        }
    }
}
