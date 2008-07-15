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
            base.Select( selection );
        }

        public override void Cancel()
        {
            base.Cancel();
        }
    }
}
