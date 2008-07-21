using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shoota.Materials
{
    class Material
    {
        #region Fields

        private string textureName;
        private Texture2D texture;

        private string effectName;
        private Effect effect;

        private EffectParameterCollection parameters;

        #endregion

        #region Properties

        public Effect Effect
        {
            get { return effect; }
            set 
            {
                if( value != null )
                {
                    effect = value;
                    parameters = effect.Parameters;
                }
            }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public EffectParameterCollection EffectParams
        {
            get { return parameters; }
        }

        #endregion

        #region Methods

        public Material( string textureName, string effectName )
        {
            this.textureName = textureName;
            this.effectName = effectName;
        }

        public void LoadContent()
        {
            try
            {
                this.texture = GameGlobals.ContentManager.Load<Texture2D>( textureName );
                this.effect = GameGlobals.ContentManager.Load<Effect>( effectName );
                if( this.effect != null )
                    this.parameters = this.effect.Parameters;
            }
            catch { }
        }

        #endregion
    }
}
