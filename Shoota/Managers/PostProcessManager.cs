using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shoota.Managers
{
    class PostProcessManager : DrawableGameComponent
    {
        private RenderTarget2D PPTarget;
        private RenderTarget2D temp;

        private Texture2D PPTexture;

        private Effect effect;

        EffectParameter pGameTime;
        EffectParameter pPixelSizeX;
        EffectParameter pPixelSizeY;

        public Texture2D ScreenTexture
        {
            get { return PPTexture; }
        }

        public Effect Effect
        {
            get { return effect; }

            set 
            {
                if( value != null )
                {
                    effect = value;
                    pGameTime = effect.Parameters["gameTime"];
                    pPixelSizeX = effect.Parameters["pixelSizeX"];
                    pPixelSizeY = effect.Parameters["pixelSizeY"];
                }
            }
        }

        public PostProcessManager(Game game) : base( game )
        {
        }

        protected override void LoadContent()
        {
            PPTarget = CloneRenderTarget( GraphicsDevice, 1 );
            PPTexture = new Texture2D( GraphicsDevice, PPTarget.Width, PPTarget.Height );
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Clones a render target.
        /// </summary>
        /// <param name="device">The graphics device to pull the settings from.</param>
        /// <param name="numberLevels">Number of levels.</param>
        /// <returns></returns>
        private static RenderTarget2D CloneRenderTarget( GraphicsDevice device, int numberLevels )
        {
            return new RenderTarget2D( device,
                device.PresentationParameters.BackBufferWidth,
                device.PresentationParameters.BackBufferHeight,
                numberLevels,
                device.DisplayMode.Format,
                device.PresentationParameters.MultiSampleType,
                device.PresentationParameters.MultiSampleQuality
            );
        }

        /// <summary>
        /// Call before rendering the scene to prepare for post processing.
        /// </summary>
        public void PreRender()
        {
            temp = (RenderTarget2D)GraphicsDevice.GetRenderTarget( 0 );
            GameGlobals.GraphicsDevice.SetRenderTarget( 0, PPTarget );
        }

        /// <summary>
        /// Call after rendering the scene to prepare for post processing.
        /// </summary>
        public void PostRender()
        {
            GameGlobals.GraphicsDevice.SetRenderTarget( 0, temp );
            PPTexture = PPTarget.GetTexture();
        }

        /// <summary>
        /// Perform the post processing.
        /// </summary>
        public void PostProcess( GameTime gameTime )
        {
            if( effect != null )
            {
                // Set the values of default parameters.
                pGameTime.SetValue( (float)gameTime.TotalGameTime.TotalSeconds );
                pPixelSizeX.SetValue( 1.0f / GameGlobals.PostProcessManager.ScreenTexture.Width );
                pPixelSizeY.SetValue( 1.0f / GameGlobals.PostProcessManager.ScreenTexture.Height );

                // Begin our post processing.
                GameGlobals.SpriteBatch.Begin( SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None );

                effect.Begin();

                // Draw once for each pass.
                foreach( EffectPass pass in effect.CurrentTechnique.Passes )
                {
                    pass.Begin();
                    GameGlobals.SpriteBatch.Draw( GameGlobals.PostProcessManager.ScreenTexture, GameGlobals.FullScreenRect, Color.Black );
                    pass.End();
                }

                effect.End();

                GameGlobals.SpriteBatch.End();
            }
            else
            {
                GameGlobals.SpriteBatch.Begin();
                GameGlobals.SpriteBatch.Draw( GameGlobals.PostProcessManager.ScreenTexture, GameGlobals.FullScreenRect, Color.White );
                GameGlobals.SpriteBatch.End();
            }

        }
    }
}
