using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shoota.Managers
{
    enum MouseButtons
    {
        M_Left,
        M_Middle,
        M_Right,
        M_X1,
        M_X2
    }

    class MouseHandler
    {
        private MouseState mouse;

        public MouseHandler()
        {
            this.mouse = Mouse.GetState();
        }

        /// <summary>
        /// Gets the current X position of the mouse.
        /// </summary>
        public int X
        {
            get { return mouse.X; }
        }

        /// <summary>
        /// Gets the current Y position of the mouse.
        /// </summary>
        public int Y
        {
            get { return mouse.Y; }
        }

        /// <summary>
        /// Get the total scroll value since game start.
        /// </summary>
        public int ScrollWheelValue
        {
            get { return mouse.ScrollWheelValue; }
        }

        /// <summary>
        /// Checks if a mouse button is down.
        /// </summary>
        /// <param name="button">MouseButton to check.</param>
        /// <returns>Boolean button is down.</returns>
        public bool IsButtonDown( MouseButtons button )
        {
            switch( button )
            {          
                case MouseButtons.M_Left:
                    return mouse.LeftButton == ButtonState.Pressed;
  
                case MouseButtons.M_Middle:
                    return mouse.MiddleButton == ButtonState.Pressed;

                case MouseButtons.M_Right:
                    return mouse.RightButton == ButtonState.Pressed;                 

                case MouseButtons.M_X1:
                    return mouse.XButton1 == ButtonState.Pressed;

                case MouseButtons.M_X2:
                    return mouse.XButton1 == ButtonState.Pressed;
            }
            return false;
        }

        /// <summary>
        /// Checks if a mouse button is up.
        /// </summary>
        /// <param name="button">MouseButton to check.</param>
        /// <returns>Boolean button is down.</returns>
        public bool IsButtonUp( MouseButtons button )
        {
            switch( button )
            {
                case MouseButtons.M_Left:
                    return this.mouse.LeftButton == ButtonState.Released;

                case MouseButtons.M_Middle:
                    return this.mouse.MiddleButton == ButtonState.Released;

                case MouseButtons.M_Right:
                    return this.mouse.RightButton == ButtonState.Released;

                case MouseButtons.M_X1:
                    return this.mouse.XButton1 == ButtonState.Released;

                case MouseButtons.M_X2:
                    return this.mouse.XButton1 == ButtonState.Released;
            }
            return false;
        }
    }
}
