using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace Shoota.Managers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ConsoleManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        #region Fields

        private Queue<string> lineBuffer;    
        private List<ConCommand> commands;

        private int maxBufferSize;

        private bool enabled;
        private string inputBuffer;
        private string drawBuffer;

        private int scrollPosition;

        private string cursorChar;
        private float cursorElapsedTime;
        private float cursorFlashTime;

        private int textPaddingLeft;
        private int textPaddingUp;
        private int inputPaddingLeft;
        private int inputPaddingUp;

        private int lines;

        private Color bgColor;
        private Color textColor;
        private Color inputBoxColor;

        #endregion

        #region Properties

        /// <summary>
        /// Is the console shown and accepting input
        /// </summary>
        public bool ConsoleEnabled
        {
            get { return enabled; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds an existing console command
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The console command found, if any.</returns>
        public ConCommand FindConCommand( string name )
        {
            foreach( ConCommand cmd in this.commands )
            {
                if( cmd.Name == name )
                    return cmd;
            }

            return null;
        }

        /// <summary>
        /// Adds a console command
        /// </summary>
        /// <param name="name">Name of the command</param>
        /// <param name="callback">A callback that will be run when the concommand is executed</param>
        public void AddConCommand( string name, ConCommandCallback callback )
        {
            ConCommand oldCmd = FindConCommand( name );

            if( oldCmd != null )
                this.commands.Remove( oldCmd );

            this.commands.Add( new ConCommand( name, callback ) );
        }

        /// <summary>
        /// Writes a line to the game console
        /// </summary>
        /// <param name="line">The line to write</param>
        public void WriteLine( string line )
        {
            if( lineBuffer.Count > maxBufferSize )
                lineBuffer.Dequeue();

            if( lineBuffer.Count - 1 == scrollPosition + lines )
                scrollPosition++;

            lineBuffer.Enqueue( line );
        }

        /// <summary>
        /// Enables input
        /// </summary>
        public void EnableConsole()
        {
            if( !enabled )
                enabled = true;

            GameGlobals.InputManager.ClearBuffer();
            this.inputBuffer = "";
        }

        /// <summary>
        /// Disables input
        /// </summary>
        public void DisableConsole()
        {
            if( enabled )
                enabled = false;
        }

        /// <summary>
        /// Toggle the console's input cycle and drawing.
        /// </summary>
        public void ToggleConsole()
        {
            if( enabled )
                DisableConsole();
            else
                EnableConsole();
        }

        /// <summary>
        /// Parses a string command, 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool ParseString( string line )
        {
            // My regex:
            // Command: ^(?<command>\b\w*\b)(?<args>.*)
            // Args: ((?<=")[^"]*(?=")|\b\w*\b)

            Regex cmdRegex = new Regex( @"^(?<command>\b\w*\b)(?<args>.*)", RegexOptions.Compiled );

            Match cmdMatch = cmdRegex.Match( line );

            string command, args;

            if( !cmdMatch.Success )
                return false;

            command = cmdMatch.Groups["command"].Value;
            args = cmdMatch.Groups["args"].Value;

            if( command.Length < 1 )
                return false;

            Regex argsRegex = new Regex( @" (?<bleah>(?<="")[^""]*(?="")|\b\w*\b)", RegexOptions.IgnoreCase | RegexOptions.Compiled );

            List<string> argList = new List<string>();

            foreach( Match argMatch in argsRegex.Matches( args ) )
            {
                argList.Add( argMatch.Value.Remove( 0, 1 ) );
            }

            ConCommand cmd = FindConCommand( command );
            if( cmd != null )
            {
                string result = cmd.Callback( argList );
                
                if( !string.IsNullOrEmpty( result ) )
                    WriteLine( result );
            }

            return true;
        }

        /// <summary>
        /// Parses a file and executes it line by line.
        /// </summary>
        /// <param name="filename">The file to load and execute.</param>
        public void ParseFile( string filename )
        {
            try
            {
                StreamReader fileStream = new StreamReader( filename );

                string line = fileStream.ReadLine();

                while( line != null )
                {
                    if( !line.StartsWith( "//" ) )
                        ParseString( line );

                    line = fileStream.ReadLine();
                }
            }
            catch {}
        }

        #endregion

        #region Default Methods

        public ConsoleManager( Game game )
            : base( game )
        {
            // TODO: Construct any child components here
            this.commands = new List<ConCommand>();
            this.lineBuffer = new Queue<string>();

            this.inputBuffer = "";
            this.maxBufferSize = 100;

            this.enabled = false;
            
            this.bgColor = new Color( 0, 0, 0, 150 );
            this.textColor = Color.DimGray;
            this.inputBoxColor = new Color( 0, 0, 0, 100 );

            this.cursorFlashTime = 0.5f;
            this.cursorChar = "|";

            this.textPaddingLeft = 10;
            this.textPaddingUp = 2;
            this.inputPaddingLeft = 10;
            this.inputPaddingUp = 2;
            this.lines = 10;

            this.scrollPosition = 0;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update( GameTime gameTime )
        {
            // TODO: Add your update code here

            if( GameGlobals.InputManager.WasKeyReleased( Keys.OemTilde ) )
            {
                ToggleConsole();              
            }

            if( enabled )
            {
                inputBuffer = GameGlobals.InputManager.StringBuffer;

                if( GameGlobals.InputManager.WasKeyReleased( Keys.Up ) )
                {
                    if( scrollPosition > 0 )
                        scrollPosition--;
                }

                if( GameGlobals.InputManager.WasKeyReleased( Keys.Down ) )
                {
                    if( scrollPosition + lines < lineBuffer.Count - 1 )
                        scrollPosition++;
                }

                int cursorPos = GameGlobals.InputManager.CursorPosition;

                cursorElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if( cursorElapsedTime > cursorFlashTime )
                {
                    if( cursorChar == " " )
                        cursorChar = "|";
                    else
                        cursorChar = " ";

                    cursorElapsedTime = 0.0f;
                }

                drawBuffer = inputBuffer.Insert( cursorPos, cursorChar );
                drawBuffer = ">>> " + drawBuffer;

                if( GameGlobals.InputManager.WasBindReleased( Binds.Enter ) )
                {
                    GameGlobals.InputManager.ClearBuffer();

                    WriteLine( ">>" + inputBuffer );

                    foreach( string cmd in inputBuffer.Split( new char[]{ ';' } ) )
                    {
                        ParseString( inputBuffer );
                    }
                }
            }

            base.Update( gameTime );
        } 

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="gameTime">A snapshot of timing values</param>
        public override void Draw( GameTime gameTime )
        {
            if( enabled )
            {
                Vector2 textSize = GameGlobals.ConsoleFontSmall.MeasureString( "T" );

                Vector2 inputBoxPos = new Vector2( 0, ( textSize.Y * ( lines + 1 ) ) + ( textPaddingUp * ( lines + 1 ) ) + 5 );

                // Draw the background.
                GameGlobals.SpriteBatch.Begin();

                GameGlobals.SpriteBatch.Draw( GameGlobals.BlankTexture, new Rectangle( 0, 0, GameGlobals.ScrW, (int)inputBoxPos.Y + (int)textSize.Y + inputPaddingUp * 2 ), bgColor );

                // Draw the input box.
                GameGlobals.SpriteBatch.Draw( GameGlobals.BlankTexture, new Rectangle( (int)inputBoxPos.X, (int)inputBoxPos.Y, GameGlobals.ScrW, (int)textSize.Y + inputPaddingUp * 2 ), bgColor );
                GameGlobals.SpriteBatch.DrawString( GameGlobals.ConsoleFontSmall, drawBuffer, new Vector2( inputPaddingLeft, inputBoxPos.Y + inputPaddingUp ), textColor );

                int j = lines;

                string[] lineBufferArray = lineBuffer.ToArray();

                for( int i = scrollPosition + lines; i >= scrollPosition; i-- )
			    {
                    if( i < lineBufferArray.Length )
                    {
                        GameGlobals.SpriteBatch.DrawString( GameGlobals.ConsoleFontSmall, lineBufferArray[i], new Vector2( textPaddingLeft, ( textSize.Y * j ) + ( textPaddingUp * j ) ), this.textColor );
                        j--;
                    }
                }

                GameGlobals.SpriteBatch.End();
            }

            base.Draw( gameTime );
        }

        #endregion
    }
}