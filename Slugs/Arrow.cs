using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    class Arrow : Drawable
    {
        Vector2 direction;
        Vector2 beginPos;
        Vector2 endPos;

        public Arrow(Texture2D texture, Vector2 position, Color color, Vector2 origin) : base(texture, position, color, origin)
        {
            position.Y = position.Y - 50f;
            Position = position;
            beginPos = position;
            endPos = position + new Vector2(0, 20);
            direction = new Vector2(0, 0.7f);
        }

        public void Update()
        {
            if (Position.Y >= endPos.Y)
                direction = new Vector2(0, -0.7f);

            if (Position.Y <= beginPos.Y)
                direction = new Vector2(0, 0.7f);

            Position += direction;
        }

        public void PostUpdate(Vector2 pos)
        {
            pos.Y = pos.Y - 50f;
            Position = pos;

            beginPos = pos;
            endPos = pos + new Vector2(0, 20);
        }

        public override void Draw()
        {
            General.sb.Draw(Texture, Position, null, Color.White, 0, Origin, 0.5f, SpriteEffects.None, 0);
        }
    }
}
