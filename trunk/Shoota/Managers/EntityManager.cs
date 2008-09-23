using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Controllers;

using Shoota.Entities;

namespace Shoota.Managers
{
    class EntityManager : DrawableGameComponent
    {
        PhysicsSimulator physicsSimulator;

        List<Entity> ents;

        bool paused = true;

        // TODO: Lua stuff:
        //          Adding Lua coded SEnts.
        //          Binding CreateEntity to Lua.

        /// <summary>
        /// Creates an entity from a class name and returns the added entity.
        /// </summary>
        /// <param name="entClass">The class name of the entity to use.</param>
        /// <returns>The created entity after it being added to the entity list.</returns>
        public Entity CreateEntity( string entClass )
        {
            Entity newEnt = null;

            try
            {
                Type entType = Type.GetType( Assembly.CreateQualifiedName( "Shoota", "Shoota.Entities." + entClass ), true );

                newEnt = (Entity)Activator.CreateInstance( entType );     
                
            }
            catch { }           

            ents.Add( newEnt );

            return newEnt;
        }

        public EntityManager(Game game) : base( game )
        {
            physicsSimulator = new PhysicsSimulator( new Vector2( 0, 300 ) );

            this.ents = new List<Entity>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if( paused )
                return;

            // Update each entity.
            foreach( Entity ent in ents )
            {
                ent.Update( gameTime );
            }

            // Step the physics simulator.
            physicsSimulator.Update( (float)gameTime.ElapsedGameTime.TotalSeconds );

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if( paused )
                return;

            foreach( Entity ent in ents )
            {
                
            }

            base.Draw(gameTime);
        }
    }
}
