using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    public class Particle : Drawable
    {
        public float BirthTime { get; set; }
        public float MaxAge { get; set; }
        public Vector2 OrginalPosition { get; set; }
        public Vector2 Accelaration { get; set; }
        public Vector2 Direction { get; set; }


        public Particle(float birthTime, float maxAge, Vector2 originalPosition,
            Vector2 accelaration, Vector2 direction, Texture2D texture, Vector2 position, Rectangle? sourceRectangle,
            Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)

            : base(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth)
        {
            BirthTime = birthTime;
            MaxAge = maxAge;
            OrginalPosition = originalPosition;
            Accelaration = accelaration;
            Direction = direction;
        }
    }
}
