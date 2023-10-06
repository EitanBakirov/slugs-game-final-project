using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Slugs.ProjectStates
{
    public class GameState : State
    {
        Color[] teamColors;

        public Drawable background;
        public int numOfPlayersInTeam;
        public int numOfTeams;
        public bool isNext;
        int TotalTeamHealth;


        public GameState(Game1 game, int numOfPlayersInTeam, int numOfTeams) : base(General.cm, General.device, game)
        {
            background = new Drawable(Dictionaries.TextureDictionary[FolderTextures.Graphics]["Desert"], new Vector2(0, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            isNext = false;
            this.numOfPlayersInTeam = numOfPlayersInTeam;
            this.numOfTeams = numOfTeams;
            TotalTeamHealth = 100 * numOfPlayersInTeam;
            General.GenerateTerrainMask();
            SetUpPlayers();
            General.cam = new Camera(General.players[0]);

            Foreground.Create(Dictionaries.TextureDictionary[FolderTextures.Graphics]["terrain3"], Dictionaries.TextureDictionary[FolderTextures.Graphics]["groundSlug"], new Rectangle(0, 0, General.screenWidth, General.screenHeight), Color.White);
            Explosion.Create(Dictionaries.TextureDictionary[FolderTextures.Graphics]["explosion"]);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, General.cam.Mat);

            background.Draw();
            Foreground.Draw();

            DrawPlayers();

            spriteBatch.End();
            spriteBatch.Begin();
            General.players[General.currentPlayer].DrawPlayTime();

            DrawText(gameTime);
            DrawTeamsTotalHealth();

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, General.cam.Mat);
            Explosion.Draw();

            spriteBatch.End();
            spriteBatch.Begin();
        }

        public void SetUpPlayers()
        {
            General.currentPlayer = 0;
            int numOfPlayers = numOfPlayersInTeam * numOfTeams;
            General.numberOfPlayers = numOfPlayers;
            Color teamColor;
            teamColors = new Color[4];
            teamColors[0] = Color.Blue;
            teamColors[1] = Color.Yellow;
            teamColors[2] = Color.Green;
            teamColors[3] = Color.Red;

            int team = 0;
            General.players = new Slug[numOfPlayers];
            for (int i = 0; i < numOfTeams; i++)
            {

                for (int j = 0; j < numOfPlayersInTeam; j++)
                {
                    teamColor = teamColors[team];
                    Vector2 playerPosition = new Vector2();
                    playerPosition.X = General.screenWidth / (numOfPlayers + 1) * (i * numOfPlayersInTeam + j + 1);
                    playerPosition.Y = 100;// General.terrainContour[(int)playerPosition.X];
                    if (i == 0 && j == 0)
                        General.players[i * numOfPlayersInTeam + j] = new Slug(Heroes.Slug, playerPosition, true, teamColor, team, Dictionaries.TextureDictionary[FolderTextures.Graphics]["rocket"], 0, 1f, 0, Keys.A, Keys.D, Keys.W, Keys.S, Keys.Enter, Keys.Q, Keys.E, false);
                    else
                        General.players[i * numOfPlayersInTeam + j] = new Slug(Heroes.Slug, playerPosition, false, teamColor, team, Dictionaries.TextureDictionary[FolderTextures.Graphics]["rocket"], 0, 1f, 0, Keys.A, Keys.D, Keys.W, Keys.S, Keys.Enter, Keys.Q, Keys.E, false);
                    General.players[i * numOfPlayersInTeam + j].Position = new Vector2(playerPosition.X, General.players[i * numOfPlayersInTeam + j].FindTheGround());
                    team++;
                    team = team % numOfTeams;
                }

            }

            General.currentTeamHealth = new int[numOfTeams];
            for (int i = 0; i < numOfTeams; i++)
            {
                General.currentTeamHealth[i] = TotalTeamHealth;
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Slug player in General.players)
                if (player.IsAlive)
                    player.UpdateState();

            if (isNext)
            {
                if (AreAllPlayersOnGround())
                {
                    General.NextPlayer();
                    isNext = false;
                }
            }

            if (General.players[General.currentPlayer].IsShot)
                HandleShootablesCollision(gameTime);

            if (Explosion.particleList.Count > 0)
                Explosion.UpdateParticles(gameTime);

            HandleWin();

            General.cam.Update();
        }

        public bool AreAllPlayersOnGround()
        {
            bool isOnGround = true;
            foreach (Slug player in General.players)
                if (player.IsAlive)
                    if (!player.IsOnGround())
                        isOnGround = false;
            return isOnGround;
        }


        public void HandleShootablesCollision(GameTime gameTime)
        {
            isNext = true;
            List<Shootable> shootablesToRemove = new List<Shootable>();

            foreach (Shootable shootable in General.players[General.currentPlayer].PWeapon.ListOfShootables)
            {
                if (!shootable.IsOutOfScreen())
                {
                    // check collisions between shootable with ground and players
                    Vector2 collisionPos = CheckShootableCollisions(gameTime, shootable);

                    if (collisionPos.X > -1)
                    {
                        General.players[General.currentPlayer].RemoveWeaponShootableAndUpdate(shootable);

                        shootable.AddExplosion(collisionPos, gameTime);

                        foreach (Slug player in General.players)
                        {
                            Vector2 explosionVector = (player.Position + new Vector2(0, -10)) - collisionPos;
                            if (explosionVector.Length() <= shootable.Damage)
                            {
                                player.UpdateHealth((int)(shootable.Damage - explosionVector.Length()));
                                player.velocityX = shootable.powerOfExplosion * explosionVector.X / Math.Abs(explosionVector.X);
                                player.velocityY = shootable.powerOfExplosion * explosionVector.Y / Math.Abs(explosionVector.Y);
                            }
                        }
                        break;
                    }
                }
                else
                {
                    General.players[General.currentPlayer].RemoveWeaponShootableAndUpdate(shootable);
                    break;
                }
            }

            if (General.players[General.currentPlayer].PWeapon.ListOfShootables.Count == 0)
                isNext = true;
            else
                isNext = false;
        }

        //public Vector2 CheckShootableCollisions(GameTime gameTime, Shootable shootable) // TODO: if there's no time but a rocket is flying in the air it should let it finish.
        //{

        //    Vector2 collisionPos = new Vector2(-1, -1);
        //    Vector2 terrainCollisionPoint = Foreground.CheckTerrainCollision(shootable);

        //    // If the shootable is in the map

        //        // If the shootable did not collide with the terrain check the collision with each player.
        //        if (terrainCollisionPoint.X == -1)
        //        {
        //            foreach (Slug player in General.players)
        //            {
        //                if (player != General.players[General.currentPlayer])
        //                {
        //                    Vector2 playerCollisionPoint = player.CheckPlayerCollision(shootable);
        //                    if (playerCollisionPoint.X > -1)
        //                    {
        //                        collisionPos = playerCollisionPoint;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            collisionPos = terrainCollisionPoint;
        //        }

        //    return collisionPos;
        //}

        public Vector2 CheckShootableCollisions(GameTime gameTime, Shootable shootable)
        {
            Vector2 playerCollision = new Vector2(-1, -1);
            Vector2 terrainCollision = Foreground.CheckTerrainCollision(shootable);

            if (terrainCollision.X == -1)
            {
                foreach (Slug slug in General.players)
                {
                    if (slug != General.players[General.currentPlayer])
                    {
                        playerCollision = slug.CheckPlayerCollision(shootable);

                        if (playerCollision.X > -1)
                            break;
                    }
                }
            }
            else
            {
                return terrainCollision;
            }

            return playerCollision;
        }

        public void DrawPlayers()
        {
            foreach (Slug player in General.players)
                if (player.IsAlive)
                    player.Draw();
        }

        public void DrawText(GameTime gameTime)
        {
            int currentAngle = (int)MathHelper.ToDegrees(General.players[General.currentPlayer].WeaponAngle);
            General.sb.DrawString(General.font, "Angle: " + currentAngle.ToString(), new Vector2(20, 20), General.players[General.currentPlayer].color, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0f);
            General.sb.DrawString(General.font, "Power: " + General.players[General.currentPlayer].Power.ToString(), new Vector2(20, 50), General.players[General.currentPlayer].color, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0f);

            int minute = gameTime.TotalGameTime.Minutes, second = gameTime.TotalGameTime.Seconds;
            string min = minute.ToString(), sec = second.ToString();
            if (minute < 10 && minute >= 0)
                min = '0' + min;

            if (second < 10 && second >= 0)
                sec = '0' + sec;

            General.sb.DrawString(General.font, min + " : " + sec, new Vector2(General.screenWidth - 100, 15), Color.White, 0f, Vector2.Zero, 1.2f, SpriteEffects.None, 0f);
        }

        public void DrawTeamsTotalHealth()
        {
            Texture2D X = Dictionaries.TextureDictionary[FolderTextures.Graphics]["X"];
            Texture2D healthLaneTexture = Dictionaries.TextureDictionary[FolderTextures.Graphics]["HealthLane"];
            for (int i = 0; i < numOfTeams; i++)
            {
                int place = (General.screenWidth - 140) / numOfTeams;
                int posX = 120 + place * i;

                Texture2D teamColorNameTexture = Dictionaries.TextureDictionary[FolderTextures.Graphics][General.teamColorString[i]];


                General.sb.Draw(teamColorNameTexture, new Rectangle(posX, 20, teamColorNameTexture.Width, teamColorNameTexture.Height - 10), Color.White);
                if (General.currentTeamHealth[i] > 0)
                {
                    double percentage = (double)General.currentTeamHealth[i] / (double)TotalTeamHealth;
                    General.sb.Draw(healthLaneTexture, new Rectangle(posX + 140, 34, (int)((healthLaneTexture.Width * percentage)), healthLaneTexture.Height - 10), teamColors[i]);
                }
                else
                {
                    General.sb.Draw(X, new Rectangle(posX + 140, 30, X.Width - 20, X.Height - 20), Color.White);
                }
            }
        }

        private void ActivateWinMode(int numOfTeam)
        {
            game.ChangeState(new EndState(game, this, numOfTeam + 1));
        }

        private void ActivateTieMode()
        {
            game.ChangeState(new EndState(game, this, 0));
        }

        private void HandleWin()
        {
            int countZeroHealth = 0;
            for (int i = 0; i < numOfTeams; i++)
            {
                if (General.currentTeamHealth[i] > 0 && CheckIsWin(i))
                    ActivateWinMode(i);
                else if (General.currentTeamHealth[i] == 0)
                    countZeroHealth++;
                else
                    break;
            }

            if (countZeroHealth == numOfTeams)
                ActivateTieMode();
        }


        private bool CheckIsWin(int numOfTeam)
        {
            for (int i = 0; i < numOfTeams; i++)
            {
                if (General.currentTeamHealth[i] > 0 && i != numOfTeam)
                    return false;
            }
            return true;
        }
    }
}
