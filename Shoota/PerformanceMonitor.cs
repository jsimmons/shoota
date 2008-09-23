using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace Shoota
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PerformanceMonitor : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private const int padding = 5;

        private double fps;
        private double frameTime;

        private double accumFrameTime;
        private int frames;

        public PerformanceMonitor( Game game )
            : base( game )
        {
            // TODO: Construct any child components here
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

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update( GameTime gameTime )
        {
            // TODO: Add your update code here
            this.frameTime = gameTime.ElapsedGameTime.TotalMilliseconds;

            if( gameTime.ElapsedGameTime.TotalSeconds != 0 )
            {
                this.accumFrameTime += gameTime.ElapsedGameTime.TotalSeconds;
                this.frames++;

                if( frames >= 10 )
                {
                    this.fps = frames / accumFrameTime;
                    accumFrameTime = 0;
                    frames = 0;
                }
            }
            base.Update( gameTime );
        }

        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw( GameTime gameTime )
        {
            Vector2 size = GameGlobals.PerfMonFont.MeasureString( "Frame time: 0000" );
            Vector2 position = new Vector2( (GameGlobals.ScrW - size.X - padding ), padding );

            GameGlobals.SpriteBatch.Begin();

            GameGlobals.SpriteBatch.DrawString( GameGlobals.PerfMonFont, "Frame time: " + frameTime.ToString("####") , position, Color.Orange );
            
            position.Y += padding + size.Y;
            GameGlobals.SpriteBatch.DrawString( GameGlobals.PerfMonFont, "FPS: " + fps.ToString( "####" ), position, Color.Orange );

            GameGlobals.SpriteBatch.End();

            base.Draw( gameTime );
        }
    }
}