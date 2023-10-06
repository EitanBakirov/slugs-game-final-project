using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Slugs
{
    public interface IFocus
    {
        Vector2 Position { get; }
    }
    public enum Heroes { Slug }
    public enum PlayerState { Stance, Run, Walk, Jump }
    public enum Tempo { Slow = 7, Medium = 5, High = 2 }
    public enum FolderTextures { Graphics, Controls }

    static class General
    {
        #region Data

        public static SpriteBatch sb;
        public static GraphicsDeviceManager gp;
        public static ContentManager cm;

        public static GraphicsDevice device;
        public static Game1 game;

        public static Camera cam;
        public static int Time = 0;

        public static SpriteFont font;

        public static int screenWidth;
        public static int screenHeight;
        public static Slug[] players;
        public static int numberOfPlayers;
        public static int currentPlayer = 0;
        public static int[] currentTeamHealth;
        public static string[] teamColorString = { "Blue", "Yellow", "Green", "Red" };

        public static Random randomizer = new Random();
        public static int[,] terrainMask;


        #endregion

        public static void NextPlayer()
        {
            General.players[General.currentPlayer].IsPlaying = false;
            Slug previousSlug = General.players[General.currentPlayer];

            General.currentPlayer = General.currentPlayer + 1;
            General.currentPlayer = General.currentPlayer % numberOfPlayers;
            while (previousSlug != General.players[General.currentPlayer] && !General.players[General.currentPlayer].IsAlive)// TODO: Fix the loop!
                General.currentPlayer = ++General.currentPlayer % numberOfPlayers;

            cam.ChangeCamFocus(General.players[General.currentPlayer]);
            General.players[General.currentPlayer].StartPlaying();
        }

        public static void GenerateTerrainMask()
        {
            terrainMask = new int[screenWidth, screenHeight];
            Color[,] terrainColorArray = TextureTo2DArray(Dictionaries.TextureDictionary[FolderTextures.Graphics]["terrain3"]);

            for (int x = 0; x < screenWidth; x++)
            {
                for (int y = 0; y < screenHeight; y++)
                {
                    if (terrainColorArray[x, y] == Color.Black)
                        terrainMask[x, y] = 1;
                    else
                        terrainMask[x, y] = 0;
                }

            }
        }

        public static Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }

       
    }
}

