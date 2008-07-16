using System;
using System.Collections.Generic;
using System.Text;

namespace Shoota.GameScreens
{
    class MainMenu : MenuBase
    {
        public MainMenu()
        {
            this.title = "Main Menu - SHOOTA";

            this.items = new List<string>();
            items.Add( "Play Game!" );
            items.Add( "Options" );
            items.Add( "Exit" );
        }
        
        public override void Select( int selection )
        {
            switch( selection )
            {
                case 0:
                    GameGlobals.ScreenManager.PushScreen( "GameScreen", true );
                    break;

                case 1:
                    GameGlobals.ScreenManager.PushScreen( "OptionsScreen", true );
                    break;

                case 2:
                    GameGlobals.ScreenManager.PushScreen( "ExitScreen", false );
                    break;
            }

            base.Select( selection );
        }

        public override void Cancel()
        {
            GameGlobals.ScreenManager.PushScreen( "ExitScreen", false );

            base.Cancel();
        }
    }
}
