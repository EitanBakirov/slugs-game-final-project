using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using Slugs.Weapons;

namespace Slugs
{
    public class Slug : Animation
    {
        #region Data
        public Heroes hero { get; }
        BaseKeys keyboard;

        public int Health { set;  get; }
        public int FallHealth { set; get; }

        Vector2 drc;
        float gravitaion;

        public float velocityX { get; set; }
        public float velocityY { get; set; }

        float accelerationY;

        public bool IsAlive { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsShot  { get; set; }

        public Weapon PWeapon { get; set; }
        public float Power { get; set; }
        public float WeaponAngle { get; set; }

        public int PlayTime { get; set; }
        public int PrepTime { get; set; }

        public int gravityActivateTime = 0;
        public int gravityDelay = 500;
        public bool canWalk;

        public int numOfTeam;
        Arrow arrow;

        private Vector2 posOfWeapon = new Vector2(3, -10);

        #endregion

        #region Ctors
        public Slug(Heroes hero, Vector2 position, bool isPlaying, Color color, int numOfTeam, Texture2D rocketTexture, float rotation, float scale, float layerDepth,
                          Keys left, Keys right, Keys up, Keys down, Keys shift, Keys Q, Keys E, bool isBot) : base(hero, PlayerState.Stance, position, color, rotation, scale, layerDepth)
        {
            this.hero = hero;
            Position = position;
            IsAlive = true;
            this.numOfTeam = numOfTeam;
            Health = 100;
            FallHealth = 0;
            IsPlaying = isPlaying;
            Power = 100;
            WeaponAngle = MathHelper.ToRadians(90);


            if (!isBot)
            {
                keyboard = new UserKeys(right, left, up, down, shift, Q, E);
                PWeapon = new Bazooka(Dictionaries.TextureDictionary[FolderTextures.Graphics]["Bazooka"], new Vector2(Position.X + 3, Position.Y - 10), WeaponAngle, null, Color.White, WeaponAngle, new Vector2(15, 36), 0.6f, this.Effects, 0);

            }
            else
            {
                keyboard = new BotKeyboard(this);
                PWeapon = new Uzi(Dictionaries.TextureDictionary[FolderTextures.Graphics]["Uzi"], new Vector2(Position.X + 3, Position.Y - 10), WeaponAngle, null, Color.White, WeaponAngle, new Vector2(15, 36), 0.6f, this.Effects, 0);

            }
            drc = Vector2.Zero;
            gravitaion = 0.5f;

            drc = new Vector2(1, 1);
            accelerationY = 0.16f;
            velocityX = 0;
            velocityY = 0;

            PlayTime = General.Time + 60000;
            PrepTime = General.Time + 3000;

            arrow = new Arrow(Dictionaries.TextureDictionary[FolderTextures.Graphics]["RedArrow"], Position, Color.White, new Vector2(32, 80));
           
        }
        #endregion

        public void RemoveWeaponShootableAndUpdate(Shootable shootable)
        {
            PWeapon.RemoveShootable(shootable);
        }

        public bool IsOnGround()
        {
            if (General.terrainMask[(int)this.Position.X, (int)this.Position.Y] == 1)
                return true;
            return false;
        }

        public void UpdateHealth(int damageTaken)
        {
            if (damageTaken >= Health)
                damageTaken = Health;

            Health -= damageTaken;
            General.currentTeamHealth[numOfTeam] -= damageTaken;
        }

        public void StartPlaying()
        {
            IsPlaying = true;
            IsShot = false;
            PlayTime = General.Time + 60000;
            PrepTime = General.Time + 3000;

            PWeapon.Reset();
        }

        public void UpdateState()
        {
            keyboard.Update();
            PWeapon.Update(Position + posOfWeapon, WeaponAngle, this.Effects);
            if (this.Health <= 0)
            {
                this.IsAlive = false;
                if (IsPlaying && !IsShot)
                    General.NextPlayer();
            }

            if (IsPlaying)
            {
                if (General.Time > PrepTime)
                {
                    Input();
                    arrow.PostUpdate(Position);

                    if (General.Time >= PlayTime && IsPlaying && !IsShot)//PRocket.IsFlying)
                    {
                        arrow.PostUpdate(Position);
                        General.NextPlayer();
                    }
                }
                else
                {
                    arrow.Update();
                }
            }
            else
                arrow.PostUpdate(Position);

            PlayerMovement();

            if (!IsPlaying)
            {
                SpriteSheetAnimation = Dictionaries.PlayerAnimationDictionary[hero][PlayerState.Stance];
            }
        }

        public void PlayerMovement()
        {
            Move();
            base.Update();
        }

        private void Input()
        {
            if (keyboard.LeftPressed())
            {
                if (Effects == SpriteEffects.None)
                    WeaponAngle = -WeaponAngle;

                if ((SpriteSheetAnimation != Dictionaries.PlayerAnimationDictionary[hero][PlayerState.Walk] || Effects == SpriteEffects.None))
                {
                    if (IsOnGround())
                    {
                        posOfWeapon = new Vector2(3, -10);
                        CurrentIndex = 0;
                        FrameDelay = 0;
                    }
                    Effects = SpriteEffects.FlipHorizontally;
                }

                if (canWalk)
                    velocityX -= 1;

                if (keyboard.SpacePressed())
                {
                    posOfWeapon = new Vector2(2, -22);
                    SpriteSheetAnimation = Dictionaries.PlayerAnimationDictionary[hero][PlayerState.Jump];
                }
                else if (IsOnGround())
                {
                    posOfWeapon = new Vector2(3, -10);
                    SpriteSheetAnimation = Dictionaries.PlayerAnimationDictionary[hero][PlayerState.Walk];
                }
            }
            else if (keyboard.RightPressed())
            {
                if (Effects == SpriteEffects.FlipHorizontally)
                    WeaponAngle = -WeaponAngle;

                if ((SpriteSheetAnimation != Dictionaries.PlayerAnimationDictionary[hero][PlayerState.Walk] || Effects == SpriteEffects.FlipHorizontally))
                {
                    if (IsOnGround())
                    {
                        posOfWeapon = new Vector2(3, -10);
                        CurrentIndex = 0;
                        FrameDelay = 0;
                    }
                    Effects = SpriteEffects.None;
                }

                if (canWalk)
                    velocityX += 1;

                if (keyboard.SpacePressed())
                {
                    posOfWeapon = new Vector2(2, -22);
                    SpriteSheetAnimation = Dictionaries.PlayerAnimationDictionary[hero][PlayerState.Jump];
                }
                else if (IsOnGround())
                {
                    posOfWeapon = new Vector2(3, -10);
                    SpriteSheetAnimation = Dictionaries.PlayerAnimationDictionary[hero][PlayerState.Walk];
                }

            }
            else if (keyboard.SpacePressed() && IsOnGround())
            {
                posOfWeapon = new Vector2(2, -22);
                SpriteSheetAnimation = Dictionaries.PlayerAnimationDictionary[hero][PlayerState.Jump];
                CurrentIndex = 0;
                FrameDelay = 0;
            }
            else if (SpriteSheetAnimation != Dictionaries.PlayerAnimationDictionary[hero][PlayerState.Stance] && IsOnGround())
            {
                posOfWeapon = new Vector2(3, -10);
                SpriteSheetAnimation = Dictionaries.PlayerAnimationDictionary[hero][PlayerState.Stance];
                CurrentIndex = 0;
                FrameDelay = 0;
            }

            if (keyboard.QPressed())
            {
                WeaponAngle -= 0.01f;
            }

            if (keyboard.EPressed())
            {
                WeaponAngle += 0.01f;
            }

            if (Effects == SpriteEffects.None)
            {
                if (WeaponAngle > MathHelper.ToRadians(180))
                    WeaponAngle = MathHelper.ToRadians(180);
                else if (WeaponAngle < 0)
                    WeaponAngle = 0;
            }
            else
            {
                if (WeaponAngle > 0)
                    WeaponAngle = 0;
                else if (WeaponAngle < MathHelper.ToRadians(-180))
                    WeaponAngle = MathHelper.ToRadians(-180);
            }

            if (keyboard.DownPressed())
            {
                Power -= 1;
            }

            if (keyboard.UpPressed())
            {
                Power += 1;
            }

            if (Power > 1000)
            {
                Power = 1000;
            }
            if (Power < 0)
            {
                Power = 0;
            }

            if (keyboard.OnePressed())
                PWeapon = new Bazooka(Dictionaries.TextureDictionary[FolderTextures.Graphics]["Bazooka"], new Vector2(Position.X + 3, Position.Y - 10), PWeapon.Angle, null, Color.White, PWeapon.Angle, new Vector2(15, 36), 0.6f, this.Effects, 0);
            else if (keyboard.TwoPressed())
                PWeapon = new Uzi(Dictionaries.TextureDictionary[FolderTextures.Graphics]["Uzi"], new Vector2(Position.X + 3, Position.Y - 10), PWeapon.Angle, null, Color.White, PWeapon.Angle, new Vector2(21, 30), 0.6f, this.Effects, 0);
            else if (keyboard.ThreePressed())
                PWeapon = new Shotgun(Dictionaries.TextureDictionary[FolderTextures.Graphics]["Shotgun"], new Vector2(Position.X + 3, Position.Y - 10), PWeapon.Angle, null, Color.White, PWeapon.Angle, new Vector2(10, 39), 0.6f, this.Effects, 0);

            if (keyboard.SpacePressed() && IsOnGround())
            {
                velocityY -= 3f;
            }

            if (keyboard.EnterPressed() && !IsShot)
            {
                IsShot = true;
                PWeapon.ShootWeapon(Position + posOfWeapon, WeaponAngle, Power);
            }
        }

        public void Move()
        {
            float startPositionY = Position.Y;
            int groundY = FindTheGround();

            if (groundY == (int)(Position.Y - Texture.Height))
                Position = new Vector2(Position.X + velocityX, Position.Y + 1);

            Gravity(groundY);

            float endPositionY = Position.Y;

            if (startPositionY < endPositionY)
                velocityX *= 2;

            else if (startPositionY > endPositionY)
                velocityX *= 0.1f;

            if (velocityY >= 6f)
                FallHealth += 1;

            Position += new Vector2(drc.X * velocityX, drc.Y * velocityY);

            if (Position.Y == groundY)
            {
                velocityX = 0;
                UpdateHealth(FallHealth);
                FallHealth = 0;
            }

        }

        private void Gravity(int groundY)
        {
            if (this.Position.Y + velocityY < groundY)
            {
                if (General.Time - gravityActivateTime >= gravityDelay)
                    canWalk = false;
                velocityY += accelerationY;
            }
            else
            {
                canWalk = true;
                Position = new Vector2(Position.X, groundY);
                velocityY = 0;
            }
        }

        public int FindTheGround()
        {
            if (Position.Y - Texture.Height > 0 && Position.X + velocityX < General.screenWidth && Position.X + velocityX > 0)
                for (int i = (int)Position.Y - Texture.Height; i < General.screenHeight; i++)
                {
                    if (General.terrainMask[(int)(Position.X + velocityX), i] == 1)
                    {
                        if (i == (int)(Position.Y - Texture.Height))
                        {
                            velocityX = 0;
                            velocityY = -velocityY;
                            
                            if (General.terrainMask[(int)(Position.X + velocityX), (int)Position.Y] == 1)
                                return (int)Position.Y;

                            return (int)Position.Y + 1;
                        }
                        return i;
                    }
                }
            velocityX = 0;
            return (int)Position.Y;
        }

        public Vector2 CheckPlayerCollision(Shootable shootable)
        {
            Vector2 playerCollisionPoint = new Vector2(-1, -1);

            if (this.IsAlive)
            {
                int xPos = (int)this.Position.X;
                int yPos = (int)this.Position.Y;

                foreach (Circle c in this.SpriteSheetAnimation.AllPagesCircle[CurrentIndex])
                {
                    Vector2 center1 = Vector2.Transform(c.Center, Matrix.CreateRotationZ(Rotation));

                    // Finding the vector of the center of the circle
                    Vector2 v1 = Position + center1 * Scale;

                    // Finding the vector of the nose of the shootable
                    Vector2 rocketNose = shootable.GetShootableNose();

                    if ((v1 - rocketNose).Length() < (c.Radius * Scale))
                        return rocketNose;

                    if ((v1 - rocketNose - shootable.Direction / 2).Length() < (c.Radius * Scale))
                        return rocketNose;

                    if ((v1 - rocketNose - shootable.Direction).Length() < (c.Radius * Scale))
                        return rocketNose;
                }
            }
            return new Vector2(-1, -1);
        }

        public bool Collision(Slug slug)
        {
            if (this.SpriteSheetAnimation.AllPagesCircle != null)
            {
                foreach(Circle c in this.SpriteSheetAnimation.AllPagesCircle[CurrentIndex])
                {
                    Vector2 center1 = Vector2.Transform(c.Center, Matrix.CreateRotationZ(Rotation));
                    Vector2 v1 = Position + center1 * Scale;
                    foreach (Circle c2 in slug.SpriteSheetAnimation.AllPagesCircle[slug.CurrentIndex])
                    {
                        Vector2 center2 = Vector2.Transform(c2.Center, Matrix.CreateRotationZ(slug.Rotation));
                        Vector2 v2 = slug.Position + center2 * slug.Scale;

                        if ((v1 - v2).Length() < (c.Radius * Scale + c2.Radius * slug.Scale))
                            return true;
                    }
                }
            }
            return false;
        }

        public override void Draw()
        {
            base.Draw();
            if (IsPlaying)
            {
                PWeapon.Draw();

                if (General.Time <= PrepTime)
                    arrow.Draw();
            }
            General.sb.DrawString(General.font, Health.ToString(), new Vector2(Position.X - 10, Position.Y - 55), color);
        }

        public void DrawPlayTime()
        {
            if (General.Time > PrepTime) 
                General.sb.DrawString(General.font, "Time Left:  " + (PlayTime - General.Time) / 1000 / 60 + " : " + (PlayTime - General.Time) / 1000, new Vector2(20, 80), color, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0f);
            else
                General.sb.DrawString(General.font, "Prep. Time: " + (PrepTime - General.Time) / 1000 / 60 + " : " + (PrepTime - General.Time) / 1000, new Vector2(20, 80), color, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0f);
        }

    }
}

