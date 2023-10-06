using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Slugs
{

    public class SpriteSheet
    {
        #region Data
        public Texture2D Tex { get; private set; }
        public List<Rectangle> Recs { get; private set; }
        public List<Vector2> Orgs { get; private set; }
        public Tempo Pace { get; private set; }

        public List<List<Circle>> AllPagesCircle { get; private set; }
        #endregion

        #region Ctors
        public SpriteSheet(Heroes hero, PlayerState state)
        {
            Recs = new List<Rectangle>();
            Orgs = new List<Vector2>();
            Tex = General.cm.Load<Texture2D>(hero + "/" + state);
            Color[] c = new Color[Tex.Width];
            List<int> pos = new List<int>();

            Tex.GetData(0, 0, new Rectangle(0, Tex.Height - 1, Tex.Width, 1), c, 0, Tex.Width);
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] != c[1])
                    pos.Add(i);
            }
            for (int i = 0; i < pos.Count - 2; i += 2)
            {
                Recs.Add(new Rectangle(pos[i], 0, pos[i + 2] - pos[i], Tex.Height - 2));
            }
            for (int i = 0; i < pos.Count - 2; i += 2)
            {
                Orgs.Add(new Vector2(pos[i + 1] - pos[i], Tex.Height - 2));
                
            }
            for (int i = 0; i < pos.Count - 2; i += 2)
            {
                CreateCircles(hero, state);
            }

            Pace = FindTempo(state);

            // Make background color transparent
            MakeTransparent();
        }
        #endregion

        void CreateCircles(Heroes hero, PlayerState pState)
        {
            Texture2D maskTexture;
            Color[] c;
            int placeInRec;
            bool foundFirst;
            Color firstC;
            Vector2 placeCenter;
            float radius;

            if (File.Exists(Directory.GetCurrentDirectory() + "/Content/" + hero + "/" + pState + "Mask" + ".xnb"))
            {
                maskTexture = General.cm.Load<Texture2D>(hero + "/" + pState + "Mask");
                AllPagesCircle = new List<List<Circle>>();
                c = new Color[maskTexture.Width * maskTexture.Height];
                maskTexture.GetData<Color>(c);
                foundFirst = false;
                firstC = new Color();
                placeCenter = new Vector2();
                radius = 0;

                for (int i = 0; i < Recs.Count; i++)
                {
                    AllPagesCircle.Add(new List<Circle>());
                    for (int j = 0; j < Recs[i].Height * Recs[i].Width; j++)
                    {
                        placeInRec = j % Recs[i].Width + Recs[i].X + j / Recs[i].Width * maskTexture.Width;

                        if (i == 0 && j == 0)
                            firstC = c[0];
                        else if (c[placeInRec] == firstC && foundFirst == false)
                        {
                            foundFirst = true;
                            placeCenter = new Vector2(placeInRec % maskTexture.Width, placeInRec / maskTexture.Width) - new Vector2(Recs[i].X + Orgs[i].X, Orgs[i].Y);
                        }
                        else if(foundFirst == true && c[placeInRec] == firstC)
                        {
                            foundFirst = false;
                            radius = (new Vector2(placeInRec % maskTexture.Width, placeInRec / maskTexture.Width) - new Vector2(Recs[i].X + Orgs[i].X, Orgs[i].Y) - placeCenter).Length();
                            AllPagesCircle[i].Add(new Circle(placeCenter, radius));
                        }
                    }
                }
            }

        }
        
        private Tempo FindTempo(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.Stance:
                    return Tempo.Slow;
                case PlayerState.Walk:
                    return Tempo.Slow;
                case PlayerState.Run:
                    return Tempo.Medium;
                default:
                    return Tempo.Slow;
            }
        }

        private void MakeTransparent()
        {
            Color[] allcolor = new Color[Tex.Width * Tex.Height];

            Tex.GetData<Color>(allcolor);
            for (int i = 1; i < allcolor.Length; i++)
            {
                if (allcolor[i] == allcolor[0])
                    allcolor[i] = Color.Transparent;
            }
            allcolor[0] = Color.Transparent;
            Tex.SetData<Color>(allcolor);
        }
    }

}
