using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Slugs
{
    class BotKeyboard : BaseKeys
    {
        Slug bot;
        Vector2 direction;
        Vector2 weaponRotation;
        float power;
        Slug target;
        float targetDist;
        bool isShot;

        public BotKeyboard(Slug bot) : base()
        {
            direction = Vector2.Zero;

            this.bot = bot;
            this.target = null;
            this.targetDist = 0;
            isShot = false;
            
        }
        public override Boolean RightPressed()
        {
            return (direction.X > 0);
        }
        public override Boolean LeftPressed()
        {
            return (direction.X < 0);
        }
        public override Boolean UpPressed()
        {
            return (power > 0);
        }
        public override Boolean DownPressed()
        {
            return (power < 0);
        }
        public override Boolean RightReleased()
        {
            return false;
        }
        public override Boolean LeftReleased()
        {
            return false;
        }
        public override Boolean UpReleased()
        {
            return false;
        }
        public override Boolean DownReleased()
        {
            return false;
        }

        public override Boolean SpacePressed()
        {
            return false;
        }
        public override bool QPressed()
        {
            return (weaponRotation.X < 0);
        }

        public override bool EPressed()
        {
            return (weaponRotation.X > 0);
        }

        public override bool QReleased()
        {
            return false;
        }

        public override bool EReleased()
        {
            return false;
        }

        public override bool EnterPressed()
        {
            if (isShot)
            {
                return true;
            }
            return false;
        }

        public override bool EnterReleased()
        {
            return false;
        }

        public override bool SpaceReleased()
        {
            return false;
        }

        public override void Update()
        {
            if (bot.IsPlaying)
            {
                FindClosestSlug();

                if (target != null)
                {
                    Vector2 shootVector = target.Position - bot.Position;
                    float angleFromVector = (float)Math.Atan2(shootVector.X, -shootVector.Y);

                    if (bot.Position.X > target.Position.X)
                    {
                        if (bot.Effects == SpriteEffects.None)
                        {
                            bot.Effects = SpriteEffects.FlipHorizontally;
                            angleFromVector = -angleFromVector;
                        }

                    }
                    else if (bot.Position.X < target.Position.X)
                    {
                        if (bot.Effects == SpriteEffects.FlipHorizontally)
                        {
                            bot.Effects = SpriteEffects.None;
                            angleFromVector = -angleFromVector;
                        }
                }

                    bot.WeaponAngle = angleFromVector;
                    isShot = true;
                }
                else
                    isShot = false;
            }
        }

        public void FindClosestSlug()
        {
            float minDist = float.MaxValue;
            float currDist;
            for (int i = 0; i < General.players.Length; i++)
            {
                if (General.players[i].numOfTeam != bot.numOfTeam && General.players[i].IsAlive)
                {
                    currDist = Math.Abs((bot.Position - General.players[i].Position).Length());
                    if (minDist > currDist)
                    {
                        minDist = currDist;
                        target = General.players[i];
                        targetDist = minDist;
                    }
                }
            }
        }

        public override bool OnePressed()
        {
            return false;
        }

        public override bool TwoPressed()
        {
            return false;
        }

        public override bool ThreePressed()
        {
            return false;
        }
    }
}