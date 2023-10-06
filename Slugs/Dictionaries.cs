using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Slugs
{
    static class Dictionaries
    {
        public static Dictionary<FolderTextures, Dictionary<string, Texture2D>> TextureDictionary;
        public static Dictionary<Heroes, Dictionary<PlayerState, SpriteSheet>> PlayerAnimationDictionary;

        public static void AnimationDicInit()
        {
            PlayerAnimationDictionary =
            new Dictionary<Heroes, Dictionary<PlayerState, SpriteSheet>>();

            foreach (Heroes hero in Enum.GetValues(typeof(Heroes)))
            {
                Dictionary<PlayerState, SpriteSheet> heroDic = new Dictionary<PlayerState, SpriteSheet>();
                foreach (PlayerState state in Enum.GetValues(typeof(PlayerState)))
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + "/Content/" + hero + "/" + state + ".xnb"))
                    {
                        heroDic.Add(state, new SpriteSheet(hero, state));
                    }
                }
                PlayerAnimationDictionary.Add(hero, heroDic);
            }
        }

        public static void TexturesDicInit()
        {
            TextureDictionary = new Dictionary<FolderTextures, Dictionary<string, Texture2D>>();

            Console.WriteLine("\n  enviroment path " + Environment.CurrentDirectory);
            string path = Directory.GetCurrentDirectory() + "/Content/ScreenAddons";
            Console.WriteLine("\n  path " + path);

            foreach (FolderTextures folder in Enum.GetValues(typeof(FolderTextures)))
            {
                string[] filesInFolder = Directory.GetFiles(path + "/" + folder);

                Dictionary<string, Texture2D> folderDic = new Dictionary<string, Texture2D>();

                int i = 0;
                while (i < filesInFolder.Length)
                {
                    filesInFolder[i] = Path.GetFileNameWithoutExtension(filesInFolder[i]);
                    Console.WriteLine("\n   filesInFolder[" + i + "] : " + filesInFolder[i]);

                    Texture2D textureInFolder = General.cm.Load<Texture2D>(path + "/" + folder + "/" + filesInFolder[i]);
                    Console.WriteLine("    t.name : " + textureInFolder.Name + "  t.ToString() : " + textureInFolder.ToString());
                    
                    folderDic.Add(filesInFolder[i], textureInFolder);
                    // next file
                    i++;
                }
                TextureDictionary.Add(folder, folderDic);
            }
        }
    }
}
