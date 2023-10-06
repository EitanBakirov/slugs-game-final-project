using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slugs.Weapons
{
    class Bazooka : Weapon
    {
        Rocket PRocket;

        public Bazooka(Texture2D texture, Vector2 position, float angle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) : base(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth)
        {
            ListOfShootables = new List<Shootable>();
            PRocket = new Rocket(Dictionaries.TextureDictionary[FolderTextures.Graphics]["rocket"], this.Position, null, Color.White, angle, new Vector2(42, 240), 0.1f, SpriteEffects.None, 1, new Vector2(0, 0), 80);

        }

        public override void ShootWeapon(Vector2 slugPos, float weaponAngle, float power)
        {
            IsShot = true;
            Rocket rocket = new Rocket(PRocket.Texture, slugPos, null, Color.White, PRocket.Angle, PRocket.Origin, PRocket.Scale, PRocket.Effects, PRocket.LayerDepth, PRocket.Direction, PRocket.Damage);

            rocket.Position = slugPos;

            rocket.Angle = weaponAngle;
            Vector2 up = new Vector2(0, -1);
            Matrix rotMatrix = Matrix.CreateRotationZ(rocket.Angle);
            rocket.Direction = Vector2.Transform(up, rotMatrix);
            rocket.Direction *= power / 50.0f;

            rocket.TimeToActivate = General.Time;

            ListOfShootables.Add(rocket);
        }

        public override void Update(Vector2 position, float weaponAngle, SpriteEffects slugEffects)
        {
            this.Effects = slugEffects;
            Position = position;
            Angle = weaponAngle;
            foreach (Shootable shootable in ListOfShootables)
                shootable.Update(position, weaponAngle);
        }
    }
}
