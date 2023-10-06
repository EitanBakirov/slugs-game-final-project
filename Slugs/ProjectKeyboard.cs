using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Slugs
{
    public abstract class BaseKeys
    {

        public abstract Boolean RightPressed();
        public abstract Boolean LeftPressed();
        public abstract Boolean UpPressed();
        public abstract Boolean DownPressed();
        public abstract Boolean RightReleased();
        public abstract Boolean LeftReleased();
        public abstract Boolean UpReleased();
        public abstract Boolean DownReleased();
        //public abstract Boolean ShiftPressed();

        public abstract Boolean QPressed();
        public abstract Boolean EPressed();
        public abstract Boolean QReleased();
        public abstract Boolean EReleased();

        public abstract Boolean EnterPressed();
        public abstract Boolean SpacePressed();
        public abstract Boolean EnterReleased();
        public abstract Boolean SpaceReleased();

        public abstract Boolean OnePressed();
        public abstract Boolean TwoPressed();
        public abstract Boolean ThreePressed();

        public abstract void Update();
    }

    public class UserKeys : BaseKeys
    {
        #region Data
        Keys right, left, up, down, enter, Q, E;
        #endregion

        #region Ctors
        public UserKeys(Keys right, Keys left, Keys up, Keys down, Keys enter, Keys q, Keys e)
        {
            this.right = right;
            this.left = left;
            this.up = up;
            this.down = down;
            this.enter = enter;
            this.Q = q;
            this.E = e;

            //this.shift = shift;
        }
        #endregion

        public override Boolean RightPressed()
        {
            return Keyboard.GetState().IsKeyDown(right);
        }
        public override Boolean LeftPressed()
        {
            return Keyboard.GetState().IsKeyDown(left);
        }
        public override Boolean UpPressed()
        {
            return Keyboard.GetState().IsKeyDown(up);
        }
        public override Boolean DownPressed()
        {
            return Keyboard.GetState().IsKeyDown(down);
        }
        public override Boolean RightReleased()
        {
            return Keyboard.GetState().IsKeyUp(right);
        }
        public override Boolean LeftReleased()
        {
            return Keyboard.GetState().IsKeyUp(left);
        }
        public override Boolean UpReleased()
        {
            return Keyboard.GetState().IsKeyUp(up);
        }
        public override Boolean DownReleased()
        {
            return Keyboard.GetState().IsKeyUp(down);
        }

        //public override Boolean ShiftPressed()
        //{
        //    return Keyboard.GetState().IsKeyDown(Keys.LeftShift);
        //}

        public override bool QPressed()
        {
            return Keyboard.GetState().IsKeyDown(Q);
        }

        public override bool EPressed()
        {
            return Keyboard.GetState().IsKeyDown(E);
        }

        public override bool QReleased()
        {
            return Keyboard.GetState().IsKeyUp(Q);
        }

        public override bool EReleased()
        {
            return Keyboard.GetState().IsKeyUp(E);
        }

        public override bool EnterPressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Enter);
        }

        public override bool SpacePressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Space);
        }

        public override bool EnterReleased()
        {
            return Keyboard.GetState().IsKeyUp(Keys.Enter);
        }

        public override bool SpaceReleased()
        {
            return Keyboard.GetState().IsKeyUp(Keys.Space);
        }

        public override void Update()
        {
            return;
        }

        public override bool OnePressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.D1);
        }

        public override bool TwoPressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.D2);
        }

        public override bool ThreePressed()
        {
            return Keyboard.GetState().IsKeyDown(Keys.D3);
        }
    }
}
