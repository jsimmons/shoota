using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using System.Threading;

using Shoota.Managers;

namespace Shoota
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Set up our game globals class with all it's wonderful fields.
            GameGlobals.GraphicsDevice = GraphicsDevice;

            GameGlobals.ContentManager = Content;

            // Create a new SpriteBatch, which can be used to draw textures.
            GameGlobals.SpriteBatch = new SpriteBatch( GraphicsDevice );

            // Load a basic texture.
            GameGlobals.MenuFont = Content.Load<SpriteFont>( "fonts/menufont" );

            // Create a new blank texture.
            GameGlobals.BlankTexture = new Texture2D( GraphicsDevice, 1, 1, 0, TextureUsage.None, SurfaceFormat.Color );
            Color[] colArray = new Color[1];
            colArray[0] = Color.White;
            GameGlobals.BlankTexture.SetData<Color>( colArray );

            GameGlobals.InputManager = new InputManager(this);

            // TODO: Not this, should load from file or something.
            GameGlobals.InputManager.Bind( Keys.W, Binds.Up );
            GameGlobals.InputManager.Bind( Keys.S, Binds.Down );
            GameGlobals.InputManager.Bind( Keys.A, Binds.Left );
            GameGlobals.InputManager.Bind( Keys.D, Binds.Right );
            GameGlobals.InputManager.Bind( Keys.Space, Binds.Jump );
            GameGlobals.InputManager.Bind( Keys.E, Binds.Use );
            GameGlobals.InputManager.Bind( Keys.Enter, Binds.Enter );
            GameGlobals.InputManager.Bind( Keys.Escape, Binds.ESC );
            GameGlobals.InputManager.Bind( MouseButtons.Left, Binds.Attack1 );
            GameGlobals.InputManager.Bind( MouseButtons.Right, Binds.Attack2 );

            this.Components.Add( GameGlobals.InputManager );       

            GameGlobals.ScreenManager = new ScreenManager(this);
            this.Components.Add( GameGlobals.ScreenManager );

            GameGlobals.ScreenManager.PushScreen( "MainMenu" , true );


            GameGlobals.EntityManager = new EntityManager( this );
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if( GameGlobals.InputManager.WasBindPressed( Binds.Attack1 ) )              
            //  this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.SlateBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
