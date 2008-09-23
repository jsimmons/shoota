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
    public class Shoota : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        public Shoota()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 854;

            graphics.SynchronizeWithVerticalRetrace = false;

            this.IsFixedTimeStep = false;         
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create the FPS counter.
            PerformanceMonitor perfmon = new PerformanceMonitor( this );
            perfmon.DrawOrder = 2;
            this.Components.Add( perfmon );

            // Create and start the input manager.
            GameGlobals.InputManager = new InputManager( this );
            this.Components.Add( GameGlobals.InputManager );

            // Create the Console and make sure it draws on top of everything else.
            GameGlobals.ConsoleManager = new ConsoleManager( this );
            GameGlobals.ConsoleManager.DrawOrder = 1;
            this.Components.Add( GameGlobals.ConsoleManager );

            // Add our post processor system.
            GameGlobals.PostProcessManager = new PostProcessManager( this );
            this.Components.Add( GameGlobals.PostProcessManager );
            
            // Create and start the screen manager.
            GameGlobals.ScreenManager = new ScreenManager( this );
            this.Components.Add( GameGlobals.ScreenManager );

            GameGlobals.EntityManager = new EntityManager( this );
            this.Components.Add( GameGlobals.EntityManager );

            GameGlobals.MaterialManager = new MaterialManager( this, Content );
            this.Components.Add( GameGlobals.MaterialManager );

            System.Reflection.AssemblyName name = System.Reflection.Assembly.GetEntryAssembly().GetName();
            GameGlobals.ConsoleManager.WriteLine( name.FullName );
            GameGlobals.ConsoleManager.WriteLine( "" );

            // Loads the autoconfiguration script.
            GameGlobals.ConsoleManager.ParseFile( "Config/autorun.conf" );

            // Load up some scripting.
            LuaManager luaManager = new LuaManager();

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

            // Load a basic fonts.
            GameGlobals.MenuFontLarge = Content.Load<SpriteFont>( "fonts/menuFontLarge" );
            GameGlobals.MenuFontSmall = Content.Load<SpriteFont>( "fonts/menuFontSmall" );
            GameGlobals.ConsoleFontLarge = Content.Load<SpriteFont>( "fonts/consoleFontLarge" );
            GameGlobals.ConsoleFontSmall = Content.Load<SpriteFont>( "fonts/consoleFontSmall" );
            GameGlobals.PerfMonFont = Content.Load<SpriteFont>( "fonts/perfmonFont" );


            // Create a new blank texture.
            GameGlobals.BlankTexture = new Texture2D( GraphicsDevice, 1, 1, 0, TextureUsage.None, SurfaceFormat.Color );
            Color[] colArray = new Color[1];
            colArray[0] = Color.White;
            GameGlobals.BlankTexture.SetData<Color>( colArray );

            // Push the main menu and go.
            GameGlobals.ScreenManager.PushScreen( "MainMenu", true );
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
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here.
     
            base.Draw(gameTime);
        }
    }
}
