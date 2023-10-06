using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using Slugs.ProjectStates;

namespace Slugs
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private State currentState;
        private State nextState;

        public void ChangeState(State state)
        {
            nextState = state;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1000;//600;
            graphics.PreferredBackBufferHeight = 700;//480;
            graphics.ApplyChanges();

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            General.font = Content.Load<SpriteFont>("FFont");

            General.device = graphics.GraphicsDevice;
            General.sb = spriteBatch;
            General.gp = graphics;
            General.cm = Content;

            Dictionaries.AnimationDicInit();
            Dictionaries.TexturesDicInit();

            General.screenWidth = General.device.PresentationParameters.BackBufferWidth;
            General.screenHeight = General.device.PresentationParameters.BackBufferHeight;

            currentState = new MenuState(this);
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            General.Time = (int)gameTime.TotalGameTime.TotalMilliseconds;

            if (nextState != null)
            {
                currentState = nextState;
                nextState = null;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentState.Update(gameTime);
            currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            currentState.Draw(gameTime, General.sb);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
