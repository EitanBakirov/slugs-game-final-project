using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Slugs
{
    public class Drawable : IFocus
    {
        #region Data
        public Texture2D Texture { get; set; }
        public Rectangle? SourceRectangle { get; set; }
        public Rectangle DestionationRectangle { get; set; }
        public Vector2 Origin { get; set; }
        public SpriteEffects Effects { get; set; }
        public Vector2 Position { get; set; }
        public Color color { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public float LayerDepth { get; set; }

        public SpriteFont font { get; set; }
        public string text { get; set; }
        #endregion

        #region Ctors
        public Drawable(Vector2 position,
                        Color color, float rotation, float scale, SpriteEffects effects, float layerDepth)
        {
            this.Position = position;
            this.color = color;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Effects = effects;
            this.LayerDepth = layerDepth;
        }

        public Drawable(Rectangle destionationRectangle, Color color)
        {
            DestionationRectangle = destionationRectangle;
            this.color = color;
        }

        public Drawable(Texture2D texture, Vector2 position, Rectangle? sourceRectangle,
                        Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            Texture = texture;
            this.Position = position;
            SourceRectangle = sourceRectangle;
            this.color = color;
            this.Rotation = rotation;
            Origin = origin;
            this.Scale = scale;
            this.Effects = effects;
            this.LayerDepth = layerDepth;
        }
        public Drawable(Color color, float rotation, float scale, float layerDepth)
        {
            this.color = color;
            this.Rotation = rotation;
            this.Scale = scale;
            this.LayerDepth = layerDepth;
        }

        public Drawable(Texture2D texture, Rectangle? sourceRectangle, Color color)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
            this.color = color;
        }

        public Drawable(Texture2D texture)
        {
            Texture = texture;
        }

        public Drawable(Vector2 position, Color color, SpriteFont font)
        {
            Position = position;
            this.color = color;
            this.font = font;
            this.text = text;
        }

        public Drawable()
        {
        }

        public Drawable(Texture2D texture, Vector2 position, Color color, Vector2 origin)
        {
            Texture = texture;
            Origin = origin;
            Position = position;
            this.color = color;
        }
        #endregion

        public virtual void Draw()
        {
            General.sb.Draw(
                    Texture, Position, SourceRectangle,
                    color, Rotation, Origin,
                    Scale, Effects, LayerDepth);
        }
    }
}