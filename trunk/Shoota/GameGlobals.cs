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

using Shoota.Managers;

namespace Shoota
{
    static class GameGlobals
    {
        public static GraphicsDevice GraphicsDevice;

        public static ContentManager ContentManager;
        public static InputManager InputManager;
        public static EntityManager EntityManager;
        public static ScreenManager ScreenManager;

        public static SpriteBatch SpriteBatch;
        public static SpriteFont MenuFont;
        public static Texture2D BlankTexture;

        public static int ScrW
        {
            get
            {
                return GraphicsDevice.Viewport.Width;
            }
        }

        public static int ScrH
        {
            get
            {
                return GraphicsDevice.Viewport.Height;
            }
        }
    }
}
