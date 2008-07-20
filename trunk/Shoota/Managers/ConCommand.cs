using System;
using System.Collections.Generic;
using System.Text;

namespace Shoota.Managers
{
    public delegate string ConCommandCallback( List<string> args );

    public class ConCommand
    {        
        /// <summary>
        /// The name of the concommand.
        /// </summary>
        public string Name;

        /// <summary>
        /// The callback to be executed when the concommand is called.
        /// </summary>
        public ConCommandCallback Callback;

        public ConCommand( string name, ConCommandCallback callback )
        {
            this.Name = name;
            this.Callback = callback;
        }
    }
}
