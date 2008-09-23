using System;

namespace Shoota
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Shoota game = new Shoota())
            {
                game.Run();
            }
        }
    }
}

