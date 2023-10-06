using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    public abstract class Weapon : Drawable
    {
        public float Angle { get; set; }
        public bool IsShot { get; set; }
        public List<Shootable> ListOfShootables;
        protected Vector2 weaponShifting;

        public Weapon(Texture2D texture, Vector2 position, Rectangle? sourceRectangle,
                    Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        : base(texture, position, sourceRectangle,
                    color, rotation, origin, scale, effects, layerDepth)
        {
        }
        
        public void RemoveShootable(Shootable shootable)
        {
            shootable.Reset();
            ListOfShootables.Remove(shootable);

            if (ListOfShootables.Count == 0)
                IsShot = false;
        }

        public void Reset()
        {
            IsShot = false;
            ListOfShootables = new List<Shootable>();
        }

        public abstract void ShootWeapon(Vector2 slugPos, float weaponAngle, float power);

        public abstract void Update(Vector2 slugPos, float weaponAngle, SpriteEffects slugEffects);

        public override void Draw()
        {
            if (IsShot)
                foreach (Shootable shootable in ListOfShootables)
                        shootable.Draw();

            General.sb.Draw(Texture, Position, null, Color.White, Angle, Origin, 0.6f, this.Effects, 0);
        }
    }
}
