using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Shoota.Managers;

namespace Shoota.GameScreens
{
    class Screen
    {
        #region Fields

        private bool markedForDelete = false;
        private bool deletePrevWhenPushed;

        #endregion

        #region Properties

        public bool MarkedForDelete
        {
            get { return this.markedForDelete; } 
        }

        public bool DeletePrevious
        {
            set { this.deletePrevWhenPushed = value; }
            get { return this.deletePrevWhenPushed; }
        }

        #endregion

        #region Methods

        public void Delete()
        {
            this.markedForDelete = true;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Screen()
        {
        }

        /// <summary>
        /// Initializes our GameScreen
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Load any content needed for our screen.
        /// </summary>
        public virtual void LoadContent()
        {
        }

        /// <summary>
        /// Unload any loaded content.
        /// </summary>
        public virtual void UnloadContent()
        {
        }

        /// <summary>
        /// Input handling should be managed here.
        /// </summary>
        /// <param name="input">The input manager object</param>
        public virtual void HandleInput( InputManager input )
        {
        }

        /// <summary>
        /// Called to update the our screen, handle input, etc.
        /// </summary>
        /// <param name="time">A snapshot of timing values.</param>
        public virtual void Update( GameTime time )
        {
        }

        /// <summary>
        /// Draws our game screen.
        /// </summary>
        /// <param name="time">A snapshot of timing values.</param>
        public virtual void Draw( GameTime time, SpriteBatch batch )
        {
        }

        #endregion
    }
}
