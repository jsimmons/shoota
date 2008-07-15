using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Shoota.GameScreens
{
    class MenuBase : Screen
    {

        #region Fields

        // Index of the currently selected list item.
        private int selection;

        // A list of items for the menu.
        protected List<string> items;

        // The title for the screen.
        protected string title;

        // Basic settings.
        protected int textPaddingLeft;
        protected int textPaddingAbove;
        protected int titlePaddingLeft;
        protected int titlePaddingAbove;

        protected Color bgColor;
        protected Color textColor;
        protected Color highColor;

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public MenuBase()
        {
            this.title = "No Title!";
            this.items = new List<string>();

            this.bgColor = new Color( 0, 0, 0, 100 );
            this.textColor = Color.Gray;
            this.highColor = Color.Green;

            this.titlePaddingAbove = 30;
            this.titlePaddingLeft = 50;

            this.textPaddingAbove = 20;
            this.textPaddingLeft = 60;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Load Content
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// Unload Content
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Handles menu item selection
        /// </summary>
        /// <param name="selection">Index of selected entry</param>
        public virtual void Select( int selection )
        {
        }

        /// <summary>
        /// Handles menu cancellation.
        /// </summary>
        public virtual void Cancel()
        {
        }

        /// <summary>
        /// Handle input
        /// </summary>
        public override void HandleInput()
        {
            // Move the selection up and down.
            if( GameGlobals.InputManager.WasBindPressed( Shoota.Managers.Binds.Up ) )
            {
                selection--;
            }
            else if( GameGlobals.InputManager.WasBindPressed( Shoota.Managers.Binds.Down ) )
            {
                selection++;
            }

            // Wrap selection if it exceeds the number of items.
            if( selection < 0 )
                selection = this.items.Count - 1;

            if( selection > this.items.Count - 1 )
                selection = 0;

            // Handle selection and cancelation events.
            if( GameGlobals.InputManager.WasBindPressed( Shoota.Managers.Binds.Enter ) )
            {
                Select( selection );
            }
            else if( GameGlobals.InputManager.WasBindPressed( Shoota.Managers.Binds.ESC ) )
            {
                Cancel();
            }

            base.HandleInput();
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="time">A snapshot of timing values</param>
        public override void Update( Microsoft.Xna.Framework.GameTime time )
        {
            base.Update( time );
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="time">A snapshot of timing values</param>
        /// <param name="batch">A spritebatch object for drawing sprites</param>
        public override void Draw( Microsoft.Xna.Framework.GameTime time, Microsoft.Xna.Framework.Graphics.SpriteBatch batch )
        {
            batch.Draw( GameGlobals.BlankTexture, new Rectangle( 0, 0, GameGlobals.ScrW, GameGlobals.ScrH ), this.bgColor );

            batch.DrawString( GameGlobals.MenuFont, this.title, new Vector2( this.titlePaddingLeft, this.titlePaddingAbove ), this.textColor );

            for( int i = 0; i < items.Count; i++ )
            {
                Vector2 size = GameGlobals.MenuFont.MeasureString( items[i] );

                float x = this.textPaddingLeft;
                float y = ( this.textPaddingAbove * i ) + ( size.Y * i ) + ( this.titlePaddingAbove * 3 );

                if( i == selection )
                {
                    batch.DrawString( GameGlobals.MenuFont, items[i], new Vector2( x, y ), this.highColor );
                    batch.DrawString( GameGlobals.MenuFont, "->", new Vector2( x - 50, y ), this.highColor );
                }
                else
                {
                    batch.DrawString( GameGlobals.MenuFont, items[i], new Vector2( x, y ), this.textColor );
                }
            }

            base.Draw( time, batch );
        }

        #endregion
    }
}

