using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Shoota.GameScreens
{
    class ExitScreen : MenuBase
    {
        public ExitScreen()
        {
            this.title = "Are you sure you want to quit?";

            this.items = new List<string>();
            items.Add( "Return to menu" );
            items.Add( "Really Exit" );

            this.bgColor = new Color( 0, 0, 0, 220 );
        }

        public override void Select( int selection )
        {
            switch( selection )
            {
                case 0:
                    // Pop the top screen off the "stack".
                    // the top screen happens to be this so we go back to the main menu / whatever was below this.
                    GameGlobals.ScreenManager.PopScreen();
                    break;

                case 1:
                    GameGlobals.ScreenManager.Game.Exit();
                    break;
            }

            base.Select( selection );
        }

        public override void Cancel()
        {
            GameGlobals.ScreenManager.PopScreen();

            base.Cancel();
        }
    }
}
