using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    public static class Foreground
    {
        public static Texture2D MaskTexture;
        static Texture2D groundTexture;
        static Color color;
        static Rectangle destinationRectangle;
        static Color[] foregroundColors;

        public static void Create(Texture2D maskTexture, Texture2D groundTex, Rectangle destinationRec, Color texColor) 
        {
            MaskTexture = new Texture2D(General.gp.GraphicsDevice, General.screenWidth, General.screenHeight);
            groundTexture = groundTex;
            destinationRectangle = destinationRec;
            color = texColor;
            CreateForeground();
        }

        public static void CreateForeground()
        {
            // Get the a 2D array of the ground texture
            Color[,] groundColors = General.TextureTo2DArray(groundTexture);
            foregroundColors = new Color[General.screenWidth * General.screenHeight];

            for (int x = 0; x < General.screenWidth; x++)
            {
                for (int y = 0; y < General.screenHeight; y++)
                {
                    // Paint the terrain mask with the ground texture.
                    if (General.terrainMask[x, y] == 1)
                        foregroundColors[x + y * General.screenWidth] = groundColors[x % groundTexture.Width, y % groundTexture.Height];
                    else
                        foregroundColors[x + y * General.screenWidth] = Color.Transparent;
                }
            }
            MaskTexture.SetData(foregroundColors);
        }

        public static Vector2 CheckTerrainCollision(Shootable shootable)
        {
            Vector2 pos = shootable.GetShootableNose();
            if ((pos.Y - 2 >= 0 && pos.Y + 2 < General.screenHeight) && (pos.X - 2 >= 0 && pos.X + 2 < General.screenWidth))
                for (int i = (int)(pos.X - 2); i < pos.X + 2; i++)
                {
                    for (int j = (int)(pos.Y - 2); j < pos.Y + 2; j++)
                    {
                        if (General.terrainMask[i, j] == 1)
                            return pos;
                    }
                }
            return new Vector2(-1, -1);
        }

        public static void AddCrater(Color[,] tex, Matrix mat)
        {
            int width = tex.GetLength(0);
            int height = tex.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tex[x, y].R > 10)
                    {
                        Vector2 imagePos = new Vector2(x, y);
                        Vector2 screenPos = Vector2.Transform(imagePos, mat);

                        int screenX = (int)screenPos.X;
                        int screenY = (int)screenPos.Y;

                        if ((screenX) > 0 && (screenX < General.screenWidth) && screenY > 0 && screenY < General.screenHeight)
                        {
                            General.terrainMask[screenX, screenY] = 0;
                            foregroundColors[screenX + screenY * General.screenWidth] = Color.Transparent;
                        }
                    }
                }
            }

            // Update the Mask texture.
            MaskTexture.SetData(foregroundColors);
        }

        public static void Draw()
        {
            General.sb.Draw(MaskTexture, destinationRectangle, color);
        }
    }
}
