using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;

namespace Slugs.ProjectStates
{
    public class EndState : State
    {

        private List<Component> components;
        private GameState gameState;
        private bool isTie = false;
        private int numOfTeamWon;

        public EndState(Game1 game, GameState gameState, int numOfTeamWon) : base(General.cm, General.device, game)
        {
            if (numOfTeamWon == 0)
                isTie = true;
            else
                this.numOfTeamWon = numOfTeamWon - 1;

            this.gameState = gameState;

            var buttonTexture = Dictionaries.TextureDictionary[FolderTextures.Controls]["Button"];
            var buttonFont = General.font;

            var newGameButton = new Button(Dictionaries.TextureDictionary[FolderTextures.Controls]["NewGameButton"])
            {
                Scale = 1f,
                Position = new Vector2(General.screenWidth / 2, 500)
            };

            newGameButton.Click += NewGameButton_Click;

            var quitGameButton = new Button(Dictionaries.TextureDictionary[FolderTextures.Controls]["QuitGameButton"])//, buttonFont)
            {
                Position = new Vector2(General.screenWidth - 100, 600),
                Scale = 1f
            };

            quitGameButton.Click += QuitGameButton_Click;

            components = new List<Component>()
            {
                newGameButton,
                quitGameButton
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, General.cam.Mat);

            gameState.background.Draw();
            Foreground.Draw();

            gameState.DrawText(gameTime);

            gameState.DrawPlayers();
            gameState.DrawTeamsTotalHealth();

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, General.cam.Mat);
            Explosion.Draw();

            spriteBatch.End();
            spriteBatch.Begin();

            spriteBatch.Draw(Dictionaries.TextureDictionary[FolderTextures.Graphics]["DarkScreen"], new Vector2(0, 0), Color.White * 0.5f);

            if (isTie)
            {
                Texture2D tieTexture = Dictionaries.TextureDictionary[FolderTextures.Graphics]["Tie"];
                spriteBatch.Draw(tieTexture, new Vector2(General.screenWidth / 2 - tieTexture.Width / 2, 50), Color.White);
            }
            else
            {
                Texture2D wonTexture = Dictionaries.TextureDictionary[FolderTextures.Graphics][General.teamColorString[numOfTeamWon] + "Won"];
                spriteBatch.Draw(wonTexture, new Vector2(General.screenWidth / 2 - wonTexture.Width / 2, 50), Color.White);
            }

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);
        }

        public void NewGameButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new MenuState(game));
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Slug player in General.players)
                if (player.IsAlive)
                    player.UpdateState();

            if (Explosion.particleList.Count > 0)
                Explosion.UpdateParticles(gameTime);

            foreach (var component in components)
                component.Update(gameTime);
        }

        public void QuitGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Exit");
            game.Exit();
        }
    }
}
