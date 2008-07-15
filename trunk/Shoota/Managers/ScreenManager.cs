using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using System.Reflection;

using Shoota.GameScreens;

namespace Shoota.Managers
{
    enum TransitionState
    {
        In,
        Out,
        None
    }

    class ScreenManager : DrawableGameComponent
    {
        #region Fields

        private List<Screen> screensToUpdate;
        private List<Screen> screensToPush;

        // The time taken to transition from one screen to another.
        private TimeSpan transitionTime = new TimeSpan( 750 );
        
        // The current transition state.
        private TransitionState transitionState;

        // The position from 0 - 100
        // 100 meaning black screen, 0 meaning no transitioning.
        private float transPosition;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the uppermost screen on the stack.
        /// </summary>
        public Screen TopScreen
        {
            get { return this.screensToUpdate[this.screensToUpdate.Count - 1]; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new screen from a string type.
        /// </summary>
        /// <param name="type">Type of screen to create.</param>
        /// <returns>The newly created GameScreen</returns>
        public Screen createScreen( string type )
        {
            Screen newScreen;

            try
            {                
                Type screenType = Type.GetType( type, true );
                newScreen = (Screen)( Activator.CreateInstance( screenType ) );
            }
            catch( Exception e )
            {
                Console.Out.WriteLine( e.Message );
                newScreen = null;
            }
            
            return newScreen;
        }

        /// <summary>
        /// Deletes a screen, internal use only.
        /// </summary>
        /// <param name="screen">Screen to delete.</param>
        public void DeleteScreen( Screen screen )
        {
            this.screensToUpdate.Remove( screen );
        }

        /// <summary>
        /// Adds a screen to the stack.
        /// </summary>
        /// <param name="screen">The screen object to push onto the stack.</param>
        /// <param name="removePrevious">Remove the previous top entry before adding this?</param>
        public void PushScreen( Screen screen, bool removePrevious )
        {
            screen.DeletePrevious = removePrevious;
            this.screensToPush.Add( screen );            
            
            this.transitionState = TransitionState.Out;
        }

        #region Generic methods

        public ScreenManager(Game game) : base( game )
        {
        }
        
        public override void Initialize()
        {
            this.screensToPush = new List<Screen>();
            this.screensToUpdate = new List<Screen>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Update the screens.

            // Create a copy of the screens to update so we can delete screens during the update process.
            Screen[] toUpdate = screensToUpdate.ToArray();

            bool updatedOne = false;

            foreach( Screen screen in toUpdate )
            {
                if( screen.MarkedForDelete )
                    DeleteScreen( screen );

                if( !updatedOne )
                {
                    screen.HandleInput( GameGlobals.InputManager );
                    updatedOne = true;
                }

                screen.Update( gameTime );
            }

            // Update the transitioning states.
            
            // TODO: Some math to update the transition time based on the current trans position, 
            // the current game time, and the total transition time.

            float changeThisFrame = this.transitionTime.Milliseconds / gameTime.ElapsedGameTime.Milliseconds; 

            switch( this.transitionState )
            {
                case TransitionState.In:

                    this.transPosition -= changeThisFrame;

                    if( this.transPosition <= 0 )
                        this.transitionState = TransitionState.None;

                    break;

                case TransitionState.Out:

                    this.transPosition += changeThisFrame;
                       
                    if( this.transPosition >= 100 )
                    {
                        // Pop the new screens to the top of the stack. Deleting old.
                        foreach( Screen scr in this.screensToPush )
                        {
                            if( scr.DeletePrevious && this.TopScreen != null )
                                this.DeleteScreen( this.TopScreen );

                            this.screensToUpdate.Add( scr );
                        }

                        this.transitionState = TransitionState.In;
                    }

                    break;

                case TransitionState.None:
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws all the gamescreens on the stack and the transistion overlay.
        /// </summary>
        /// <param name="gameTime">A snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            GameGlobals.SpriteBatch.Begin( SpriteBlendMode.Additive, SpriteSortMode.Deferred, SaveStateMode.None );

            // Draw all our game states.
            foreach( Screen screen in screensToUpdate )
            {
                screen.Draw( gameTime, GameGlobals.SpriteBatch );
            }

            // Draw the transition overlay.

            // TODO: Calculate alpha from current transition position.
            byte alpha = (byte)((this.transPosition / 100) * 255);
    
            GameGlobals.SpriteBatch.Draw( GameGlobals.BlankTexture, new Rectangle( 0, 0, GameGlobals.ScrW, GameGlobals.ScrH ), new Color( 0, 0, 0, alpha ) );
          
            GameGlobals.SpriteBatch.End();

            base.Draw( gameTime );
        }

        #endregion
        #endregion
    }
}
