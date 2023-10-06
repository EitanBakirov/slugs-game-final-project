using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    public class Animation : Drawable
    {
        #region Data
        public SpriteSheet SpriteSheetAnimation { get; set; }
        public int CurrentIndex { get; set; }
        public int FrameDelay { get; set; }
        #endregion

        #region Ctors
        public Animation(Heroes hero, PlayerState state, Vector2 position,
                        Color color, float rotation, float scale, SpriteEffects effects, float layerDepth) :
                        base(position, color, rotation, scale, effects, layerDepth)
        {
            SpriteSheetAnimation = Dictionaries.PlayerAnimationDictionary[hero][state];
            CurrentIndex = 0;
            FrameDelay = 0;
            Texture = SpriteSheetAnimation.Tex;
        }
        public Animation(Heroes hero, PlayerState state, Vector2 position, Color color, float rotation, float scale, float layerDepth) :
                        base(color, rotation, scale, layerDepth)
        {
            SpriteSheetAnimation = Dictionaries.PlayerAnimationDictionary[hero][state];
            CurrentIndex = 0;
            FrameDelay = 0;
            Texture = SpriteSheetAnimation.Tex;
        }
        #endregion

        public void Update()
        {
            Texture = SpriteSheetAnimation.Tex;
            SourceRectangle = SpriteSheetAnimation.Recs[CurrentIndex];
            if (Effects == SpriteEffects.FlipHorizontally)
                Origin = new Vector2(SourceRectangle.Value.Width - SpriteSheetAnimation.Orgs[CurrentIndex].X, SpriteSheetAnimation.Orgs[CurrentIndex].Y);
            else
                Origin = SpriteSheetAnimation.Orgs[CurrentIndex];
            if ((int)SpriteSheetAnimation.Pace < ++FrameDelay)
            {
                FrameDelay = 0;
                CurrentIndex++;
                CurrentIndex %= SpriteSheetAnimation.Recs.Count;
            }
        }
    }
}
