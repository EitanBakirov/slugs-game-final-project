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
    public class MenuState : State
    {
        private List<Component> components;
        public List<Button> teamButtons;
        public List<Button> playerButtons;
        private int numOfTeams = 2;
        private int numOfPlayers = 2;

        public MenuState(Game1 game) : base(General.cm, General.device, game)
        {

            var buttonTexture = Dictionaries.TextureDictionary[FolderTextures.Controls]["Button"];
            var buttonFont = General.font;

            var twoButton = new Button(buttonTexture, General.font)
            {
                Scale = 3f,
                Position = new Vector2(650, 300),
                Text = "2",
            };

            twoButton.Click += TwoGameButton_Click;

            var threeButton = new Button(buttonTexture, General.font)
            {
                Scale = 3f,
                Position = new Vector2(700, 300),
                Text = "3",
            };

            threeButton.Click += ThreeGameButton_Click;

            var fourButton = new Button(buttonTexture, General.font)
            {
                Scale = 3f,
                Position = new Vector2(750, 300),
                Text = "4",
            };

            fourButton.Click += FourGameButton_Click;

            var newGameButton = new Button(Dictionaries.TextureDictionary[FolderTextures.Controls]["StartGameButton"])
            {
                Scale = 1f,
                Position = new Vector2(General.screenWidth / 2, 450)
            };

            newGameButton.Click += NewGameButton_Click;

            var quitGameButton = new Button(Dictionaries.TextureDictionary[FolderTextures.Controls]["QuitGameButton"])//, buttonFont)
            {
                Position = new Vector2(General.screenWidth / 2, 550),
                Scale = 1f
            };

            quitGameButton.Click += QuitGameButton_Click;

            components = new List<Component>()
            {
                newGameButton,
                quitGameButton
            };

            teamButtons = new List<Button>()
            {
                twoButton,
                threeButton,
                fourButton
            };

            Button twoB = new Button(twoButton);
            Vector2 pos = twoButton.Position;
            pos.Y = 350;
            twoB.Position = pos;

            Button threeB = new Button(threeButton);
            pos = threeB.Position;
            pos.Y = 350;
            threeB.Position = pos;

            Button fourB = new Button(fourButton);
            pos = fourB.Position;
            pos.Y = 350;
            fourB.Position = pos;

            twoB.Click += TwoPGameButton_Click;
            threeB.Click += ThreePGameButton_Click;
            fourB.Click += FourPGameButton_Click;

            playerButtons = new List<Button>()
            {
                twoB,
                threeB,
                fourB
            };

            twoButton.Clicked = true;
            twoButton.color = Color.Gray;

            twoB.Clicked = true;
            twoB.color = Color.Gray;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Dictionaries.TextureDictionary[FolderTextures.Graphics]["SlugsWallpaper"], new Vector2(0, 0), Color.White);
            spriteBatch.Draw(Dictionaries.TextureDictionary[FolderTextures.Graphics]["SlugsLogo"], new Vector2(350, 50), Color.White);
            spriteBatch.Draw(Dictionaries.TextureDictionary[FolderTextures.Controls]["NumOfTeams"], new Vector2(260, 270), Color.White);
            spriteBatch.Draw(Dictionaries.TextureDictionary[FolderTextures.Controls]["NumOfPlayers"], new Vector2(260, 330), Color.White);

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            foreach (var button in teamButtons)
                button.Draw(gameTime, spriteBatch);

            foreach (var button in playerButtons)
                button.Draw(gameTime, spriteBatch);

        }

        public void NewGameButton_Click(object sender, EventArgs e)
        {
            game.ChangeState(new GameState(game, numOfPlayers, numOfTeams));
        }

        public void TwoGameButton_Click(object sender, EventArgs e)
        {
            foreach (var button in teamButtons)
                if (button.Text == "2")
                {
                    button.color = Color.Gray;
                    button.Clicked = true;
                    numOfTeams = 2;
                }
                else
                {
                    button.color = Color.White;
                    button.Clicked = false;
                }
        }

        public void ThreeGameButton_Click(object sender, EventArgs e)
        {
            foreach (var button in teamButtons)
                if (button.Text == "3")
                {
                    button.color = Color.Gray;
                    button.Clicked = true;
                    numOfTeams = 3;
                }
                else
                {
                    button.color = Color.White;
                    button.Clicked = false;
                }
        }

        public void FourGameButton_Click(object sender, EventArgs e)
        {
            foreach (var button in teamButtons)
                if (button.Text == "4")
                {
                    button.color = Color.Gray;
                    button.Clicked = true;
                    numOfTeams = 4;
                }
                else
                {
                    button.color = Color.White;
                    button.Clicked = false;
                };
        }

        public void TwoPGameButton_Click(object sender, EventArgs e)
        {
            foreach (var button in playerButtons)
                if (button.Text == "2")
                {
                    button.color = Color.Gray;
                    button.Clicked = true;
                    numOfPlayers = 2;
                }
                else
                {
                    button.color = Color.White;
                    button.Clicked = false;
                }
        }

        public void ThreePGameButton_Click(object sender, EventArgs e)
        {
            foreach (var button in playerButtons)
                if (button.Text == "3")
                {
                    button.color = Color.Gray;
                    button.Clicked = true;
                    numOfPlayers = 3;
                }
                else
                {
                    button.color = Color.White;
                    button.Clicked = false;
                }
        }

        public void FourPGameButton_Click(object sender, EventArgs e)
        {
            foreach (var button in playerButtons)
                if (button.Text == "4")
                {
                    button.color = Color.Gray;
                    button.Clicked = true;
                    numOfPlayers = 4;
                }
                else
                {
                    button.color = Color.White;
                    button.Clicked = false;
                };
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
                component.Update(gameTime);

            foreach (var button in teamButtons)
                button.Update(gameTime);

            foreach (var button in playerButtons)
                button.Update(gameTime);
        }

        public void QuitGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Exit");
            game.Exit();
        }
    }
}
