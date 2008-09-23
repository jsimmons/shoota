using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Shoota.Materials;

namespace Shoota.Entities
{
    abstract class Entity
    {
        #region Fields
        /// <summary>
        /// The material which this entity will use to render.
        /// </summary>
        protected Material material;

        /// <summary>
        /// The position at which this entity will render. The top right of the entity in screen coords.
        /// </summary>
        protected Vector2 position;

        /// <summary>
        /// The center of the entity, as an offset from 0,0 where 0,0 is the upper left of the texture.
        /// </summary>
        protected Vector2 center;

        /// <summary>
        /// A rotation to apply to the entity.
        /// </summary>
        protected float rotation;
        #endregion

        #region Properties

        public Material Material
        {
            get { return this.material; }
            set { this.material = value; }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Vector2 Center
        {
            get { return this.center; }
            set { this.center = value; }
        }

        public float Rotation
        {
            get { return this.rotation; }
            set { this.rotation = value; }
        }

        #endregion

        #region Methods

        public virtual void Update( GameTime gameTime )
        {
            // Do fucking shit etc.
        }

        public virtual void Draw( GameTime gameTime, SpriteBatch batch )
        {
            if( material.Effect != null )
                material.Effect.Begin();

            foreach( EffectPass pass in material.Effect.CurrentTechnique.Passes )
            {
                pass.Begin();

                batch.Draw( material.Texture, position, material.CurrentFrame, material.Color, rotation, center, 1.0f, SpriteEffects.None, 0.5f );

                pass.End();
            }

            if( material.Effect != null )
                material.Effect.End();
        }

        #endregion

    }
}
