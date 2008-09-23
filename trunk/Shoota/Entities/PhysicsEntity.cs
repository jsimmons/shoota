using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Collisions;

namespace Shoota.Entities
{
    class PhysEnt : Entity
    {
        // The Farseer body representing this entity
        Body[] body;

        // The Farseer geometry representing this entity.
        Geom[] geom;
      
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime )
        {
            base.Update( gameTime );
        }

        public override void Draw( Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch batch )
        {
            base.Draw( gameTime, batch );
        }

    }
}
