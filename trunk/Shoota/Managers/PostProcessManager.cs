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

        public Texture2D ScreenTexture
        {
            get { return PPTexture; }
        }

        public Effect Effect
        {
            get { return effect; }
            set { effect = value; }
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

        public static RenderTarget2D CloneRenderTarget( GraphicsDevice device, int numberLevels )
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

        public void PreRender()
        {
            temp = (RenderTarget2D)GraphicsDevice.GetRenderTarget( 0 );
            GameGlobals.GraphicsDevice.SetRenderTarget( 0, PPTarget );
        }

        public void PostRender()
        {
            GameGlobals.GraphicsDevice.SetRenderTarget( 0, temp );
            PPTexture = PPTarget.GetTexture();
        }
    }
}
