using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;

using LuaInterface;

namespace Shoota.Managers
{
    class LuaManager
    {
        Lua lua;

        public LuaManager()
        {
            /*
            lua = new Lua();

            lua.RegisterFunction( "print", GameGlobals.ConsoleManager, GameGlobals.ConsoleManager.GetType().GetMethod( "WriteLine" ) );

            try
            {
                lua.DoFile( "test.lua" );
            }
            catch( LuaException e )
            {
                GameGlobals.ConsoleManager.WriteLine( e.Message );
            }
            */
        }
    }
}
