﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slugs.Weapons
{
    class Shotgun : Weapon
    {
        Bullet PBullet;

        public Shotgun(Texture2D texture, Vector2 position, float angle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) : base(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth)
        {
            ListOfShootables = new List<Shootable>();
            Position = new Vector2(position.X + 3, position.Y - 10);
            PBullet = new Bullet(Dictionaries.TextureDictionary[FolderTextures.Graphics]["ShotgunBullet"], this.Position, null, Color.White, angle, Vector2.Zero, 1f, SpriteEffects.None, 1, new Vector2(0, 0), 50);
        }

        public override void ShootWeapon(Vector2 slugPos, float weaponAngle, float power)
        {
            IsShot = true;
            int timeToActivate = General.Time;
            float angle = weaponAngle - 0.04f;
            for (int i = 0; i < 3; i++)
            {
                Bullet bullet = new Bullet(PBullet.Texture, slugPos, null, Color.White, PBullet.Angle, PBullet.Origin, PBullet.Scale, PBullet.Effects, PBullet.LayerDepth, PBullet.Direction, PBullet.Damage);

                bullet.Position = slugPos;

                bullet.Angle = angle;
                Vector2 up = new Vector2(0, -1);
                Matrix rotMatrix = Matrix.CreateRotationZ(bullet.Angle);
                bullet.Direction = Vector2.Transform(up, rotMatrix);
                bullet.Direction *= 6f;

                bullet.TimeToActivate = timeToActivate;

                ListOfShootables.Add(bullet);
                angle += 0.04f;
            }

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
