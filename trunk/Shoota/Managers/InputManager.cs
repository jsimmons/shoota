using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shoota.Managers
{
    enum Binds
    {
        Up,
        Down,
        Left,
        Right,
        Jump,
        Attack1,
        Attack2,
        Use,
        Enter,
        ESC
    }

    class InputManager : GameComponent
    {
        #region Fields
        private MouseHandler currMouseState;
        private MouseHandler prevMouseState;
        private KeyboardState currKeyboardState;
        private KeyboardState prevKeyboardState;

        private Dictionary<Binds, Keys> keyBindings;
        private Dictionary<Binds, MouseButtons> mouseBindings;
        #endregion

        #region Properties

        /// <summary>
        /// Get the current mouse state.
        /// </summary>
        public MouseHandler CurrMouseState
        {
            get { return this.currMouseState; }
        }

        /// <summary>
        /// Get the previous mouse state.
        /// </summary>
        public MouseHandler PrevMouseState
        {
            get { return this.prevMouseState; }
        }

        /// <summary>
        /// Get the current keyboard state.
        /// </summary>
        public KeyboardState CurrKeyboardState
        {
            get { return this.currKeyboardState; }
        }

        /// <summary>
        /// Get the previous keyboard state.
        /// </summary>
        public KeyboardState PrevKeyboardState
        {
            get { return this.prevKeyboardState; }
        }

        /// <summary>
        /// Returns a vector describing the current mouse position in screen coords.
        /// </summary>
        public Vector2 MousePosition
        {
            get { return new Vector2( this.currMouseState.X, this.currMouseState.Y ); }
        }

        /// <summary>
        /// Returns a vector describing the previous mouse position in screen coords.
        /// </summary>
        public Vector2 PrevMousePosition
        {
            get { return new Vector2( this.prevMouseState.X, this.prevMouseState.Y ); }
        }

        /// <summary>
        /// Returns a vector describing the mouses displacement over the last frame.
        /// </summary>
        public Vector2 MouseVelocity
        {
            get { return MousePosition - PrevMousePosition; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Binds a key to a bind.
        /// </summary>
        /// <param name="key">The key to use for the binding</param>
        /// <param name="bind">The bind to map the key to</param>
        public void Bind(Keys key, Binds bind)
        {
            this.mouseBindings.Remove( bind );
            this.keyBindings[bind] = key;
        }

        /// <summary>
        /// Binds a mouse button to a bind.
        /// </summary>
        /// <param name="button">Mouse button to bind.</param>
        /// <param name="bind">The bind the key should be mapped to.</param>
        public void Bind( MouseButtons button, Binds bind )
        {
            this.keyBindings.Remove( bind );
            this.mouseBindings[bind] = button;
        }

        /// <summary>
        /// Checks if the given bind is down this frame.
        /// </summary>
        /// <param name="bind">Bind to check</param>
        /// <returns>Boolean succeded</returns>
        public bool IsBindDown(Binds bind)
        {
            if( keyBindings.ContainsKey( bind ) )
            {
                return currKeyboardState.IsKeyDown( keyBindings[bind] );
            }
            else if( mouseBindings.ContainsKey( bind ))
            {
                return CurrMouseState.IsButtonDown( mouseBindings[bind] );
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the given bind was just pressed this frame.
        /// </summary>
        /// <param name="bind">Bind to check</param>
        /// <returns>Boolean succeded</returns>
        public bool WasBindPressed(Binds bind)
        {
            if( keyBindings.ContainsKey( bind ) )
            {
                return prevKeyboardState.IsKeyDown( keyBindings[bind] ) && currKeyboardState.IsKeyUp( keyBindings[bind] );
            }
            else if( mouseBindings.ContainsKey( bind ) )
            {
                return prevMouseState.IsButtonDown( mouseBindings[bind] ) && currMouseState.IsButtonUp( mouseBindings[bind] );
            }
            else
            {
                return false;
            }
        }

        #region DefaultMethods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game">The game object.</param>
        public InputManager(Game game) : base( game )
        {
            this.currKeyboardState = new KeyboardState();
            this.prevKeyboardState = new KeyboardState();
            this.currMouseState = new MouseHandler();
            this.prevMouseState = new MouseHandler();

            this.keyBindings = new Dictionary<Binds, Keys>();
            this.mouseBindings = new Dictionary<Binds, MouseButtons>();

            this.Bind( Keys.A, Binds.Left );
        }

        /// <summary>
        /// Updates the input manager.
        /// </summary>
        /// <param name="gameTime">A snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            prevMouseState = currMouseState;
            currMouseState = new MouseHandler();

            prevKeyboardState = currKeyboardState;
            currKeyboardState = Keyboard.GetState();

            base.Update(gameTime);
        }
        #endregion
        #endregion
    }
}
