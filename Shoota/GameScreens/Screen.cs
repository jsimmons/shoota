using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shoota.GameScreens
{
    class Screen
    {
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
        public virtual void Draw( GameTime time )
        {
        }
    }
}
