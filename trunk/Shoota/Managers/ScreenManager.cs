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
    class ScreenManager : DrawableGameComponent
    {
        private List<Screen> screens;

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
                newScreen = null;
            }
            
            return newScreen;
        }

        public ScreenManager(Game game) : base( game )
        {
        }
        
        public override void Initialize()
        {
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
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}
