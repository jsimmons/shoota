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

        private string stringBuffer;
        private int cursorPosition;
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

        /// <summary>
        /// The string buffer.
        /// </summary>
        public string StringBuffer
        {
            get { return stringBuffer; }
        }

        /// <summary>
        /// The current cursor position.
        /// </summary>
        public int CursorPosition
        {
            get { return cursorPosition; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Binds based on a list of strings.
        /// For use by the Console Commands.
        /// </summary>
        /// <param name="args">A list of string arguments, the first being the bind the second being the button to bind</param>
        /// <returns>A result string for the console command</returns>
        public string Bind( List<string> args )
        {
            string usage = "Usage: bind <Bind Enumeration> <Mouse or Key Enumeration>";

            if( args.Count < 2 )
                return usage;

            string bindStr = args[0];
            string buttonStr = args[1];

            Binds bind;

            try
            {
                bind = (Binds)Enum.Parse( typeof( Binds ), bindStr, true );
            }
            catch
            {
                return usage;
            }

            try
            {
                Keys key = (Keys)Enum.Parse( typeof( Keys ), buttonStr, true );
                Bind( key, bind );
                return "Key " + buttonStr + " successfully bound to " + bindStr + ".";
            }
            catch {}

            try
            {
                MouseButtons button = (MouseButtons)Enum.Parse( typeof( MouseButtons ), buttonStr, true );
                Bind( button, bind );
                return "MouseButton " + buttonStr + " successfully bound to " + bindStr + ".";
            }
            catch
            {
                return usage;
            }
        }

        /// <summary>
        /// Binds a key to a bind.
        /// </summary>
        /// <param name="key">The key to use for the binding</param>
        /// <param name="bind">The bind to map the key to</param>
        public void Bind(Keys key, Binds bind)
        {
            this.mouseBindings.Remove( bind );
            this.keyBindings[bind] = key;
            this.cursorPosition = 0;
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
        /// Checks if a given key was released this frame.
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Is the key down</returns>
        public bool WasKeyReleased( Keys key )
        {
            return prevKeyboardState.IsKeyDown( key ) && currKeyboardState.IsKeyUp( key );
        }

        /// <summary>
        /// Checks if the given bind was just released this frame.
        /// </summary>
        /// <param name="bind">Bind to check</param>
        /// <returns>Boolean succeded</returns>
        public bool WasBindReleased(Binds bind)
        {
            if( keyBindings.ContainsKey( bind ) )
            {
                return WasKeyReleased( keyBindings[bind] );
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

        /// <summary>
        /// Clear the string buffer.
        /// </summary>
        public void ClearBuffer()
        {
            stringBuffer = "";
            cursorPosition = 0;
        }

        /// <summary>
        /// Get the characters pressed this frame.
        /// </summary>
        /// <returns>The characters pressed this frame.</returns>
        public string GetCharacters()
        {
            string chars = "";

            bool Shifted = ( currKeyboardState.IsKeyDown( Keys.LeftShift ) || currKeyboardState.IsKeyDown( Keys.RightShift ) );

            if( WasKeyReleased( Keys.A ) ) { if( Shifted ) chars = "A"; else chars = "a"; }
            if( WasKeyReleased( Keys.B ) ) { if( Shifted ) chars = "B"; else chars = "b"; }
            if( WasKeyReleased( Keys.C ) ) { if( Shifted ) chars = "C"; else chars = "c"; }
            if( WasKeyReleased( Keys.D ) ) { if( Shifted ) chars = "D"; else chars = "d"; }
            if( WasKeyReleased( Keys.E ) ) { if( Shifted ) chars = "E"; else chars = "e"; }
            if( WasKeyReleased( Keys.F ) ) { if( Shifted ) chars = "F"; else chars = "f"; }
            if( WasKeyReleased( Keys.G ) ) { if( Shifted ) chars = "G"; else chars = "g"; }
            if( WasKeyReleased( Keys.H ) ) { if( Shifted ) chars = "H"; else chars = "h"; }
            if( WasKeyReleased( Keys.I ) ) { if( Shifted ) chars = "I"; else chars = "i"; }
            if( WasKeyReleased( Keys.J ) ) { if( Shifted ) chars = "J"; else chars = "j"; }
            if( WasKeyReleased( Keys.K ) ) { if( Shifted ) chars = "K"; else chars = "k"; }
            if( WasKeyReleased( Keys.L ) ) { if( Shifted ) chars = "L"; else chars = "l"; }
            if( WasKeyReleased( Keys.M ) ) { if( Shifted ) chars = "M"; else chars = "m"; }
            if( WasKeyReleased( Keys.N ) ) { if( Shifted ) chars = "N"; else chars = "n"; }
            if( WasKeyReleased( Keys.O ) ) { if( Shifted ) chars = "O"; else chars = "o"; }
            if( WasKeyReleased( Keys.P ) ) { if( Shifted ) chars = "P"; else chars = "p"; }
            if( WasKeyReleased( Keys.Q ) ) { if( Shifted ) chars = "Q"; else chars = "q"; }
            if( WasKeyReleased( Keys.R ) ) { if( Shifted ) chars = "R"; else chars = "r"; }
            if( WasKeyReleased( Keys.S ) ) { if( Shifted ) chars = "S"; else chars = "s"; }
            if( WasKeyReleased( Keys.T ) ) { if( Shifted ) chars = "T"; else chars = "t"; }
            if( WasKeyReleased( Keys.U ) ) { if( Shifted ) chars = "U"; else chars = "u"; }
            if( WasKeyReleased( Keys.V ) ) { if( Shifted ) chars = "V"; else chars = "v"; }
            if( WasKeyReleased( Keys.W ) ) { if( Shifted ) chars = "W"; else chars = "w"; }
            if( WasKeyReleased( Keys.X ) ) { if( Shifted ) chars = "X"; else chars = "x"; }
            if( WasKeyReleased( Keys.Y ) ) { if( Shifted ) chars = "Y"; else chars = "y"; }
            if( WasKeyReleased( Keys.Z ) ) { if( Shifted ) chars = "Z"; else chars = "z"; }

            if( WasKeyReleased( Keys.D1 ) ) { if( Shifted ) chars = "!"; else chars = "1"; }
            if( WasKeyReleased( Keys.D2 ) ) { if( Shifted ) chars = "@"; else chars = "2"; }
            if( WasKeyReleased( Keys.D3 ) ) { if( Shifted ) chars = "#"; else chars = "3"; }
            if( WasKeyReleased( Keys.D4 ) ) { if( Shifted ) chars = "$"; else chars = "4"; }
            if( WasKeyReleased( Keys.D5 ) ) { if( Shifted ) chars = "%"; else chars = "5"; }
            if( WasKeyReleased( Keys.D6 ) ) { if( Shifted ) chars = "^"; else chars = "6"; }
            if( WasKeyReleased( Keys.D7 ) ) { if( Shifted ) chars = "&"; else chars = "7"; }
            if( WasKeyReleased( Keys.D8 ) ) { if( Shifted ) chars = "*"; else chars = "8"; }
            if( WasKeyReleased( Keys.D9 ) ) { if( Shifted ) chars = "("; else chars = "9"; }
            if( WasKeyReleased( Keys.D0 ) ) { if( Shifted ) chars = ")"; else chars = "0"; }

            if( WasKeyReleased( Keys.OemTilde ) )           { if( Shifted ) chars = "~"; else chars = "`"; }
            if( WasKeyReleased( Keys.OemSemicolon ) )       { if( Shifted ) chars = ":"; else chars = ";"; }
            if( WasKeyReleased( Keys.OemQuotes ) )          { if( Shifted ) chars = "\""; else chars = "'"; }
            if( WasKeyReleased( Keys.OemQuestion ) )        { if( Shifted ) chars = "?"; else chars = "/"; }
            if( WasKeyReleased( Keys.OemPlus ) )            { if( Shifted ) chars = "+"; else chars = "="; }
            if( WasKeyReleased( Keys.OemPipe ) )            { if( Shifted ) chars = "|"; else chars = "\\"; }
            if( WasKeyReleased( Keys.OemPeriod ) )          { if( Shifted ) chars = ">"; else chars = "."; }
            if( WasKeyReleased( Keys.OemOpenBrackets ) )    { if( Shifted ) chars = "{"; else chars = "["; }
            if( WasKeyReleased( Keys.OemCloseBrackets ) )   { if( Shifted ) chars = "}"; else chars = "]"; }
            if( WasKeyReleased( Keys.OemMinus ) )           { if( Shifted ) chars = "_"; else chars = "-"; }
            if( WasKeyReleased( Keys.OemComma ) )           { if( Shifted ) chars = "<"; else chars = ","; }
            if( WasKeyReleased( Keys.OemComma ) )           { if( Shifted ) chars = "<"; else chars = ","; }
            if( WasKeyReleased( Keys.Space ) )              { chars = " "; }

            return chars;
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

            this.stringBuffer = "";
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

            if( WasKeyReleased( Keys.Back ) )
            {
                if( stringBuffer.Length > 0 )
                {
                    stringBuffer = stringBuffer.Remove( cursorPosition - 1, 1 );
                    cursorPosition--;
                }
            }

            if( WasKeyReleased( Keys.Left ) )
            {
                cursorPosition--;
                if( cursorPosition < 0 )
                    cursorPosition = 0;
            }

            if( WasKeyReleased( Keys.Right ) )
            {
                cursorPosition++;
                if( cursorPosition > stringBuffer.Length )
                    cursorPosition = stringBuffer.Length;
            }

            string chars = GetCharacters();

            if( stringBuffer.Length < 105 )
            {
                stringBuffer = stringBuffer.Insert( cursorPosition, chars );
                cursorPosition += chars.Length;
            }

            base.Update(gameTime);
        }
        #endregion
        #endregion
    }
}
