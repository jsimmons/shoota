using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

using Shoota.Materials;

namespace Shoota.Managers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 
    
    public class MaterialManager : Microsoft.Xna.Framework.GameComponent
    {
        private const string baseFolder = "Content/materials/";        

        private Dictionary<string,Material> materials;

        private List<Material> materialList;
        private List<Material> materialDeleteList;

        private ContentManager Content
        {
            get;
            set;
        }

        public MaterialManager( Game game, ContentManager content )
            : base( game )
        {            
            materials = new Dictionary<string,Material>();
            materialList = new List<Material>();
            materialDeleteList = new List<Material>();

            DirectoryInfo directory = new DirectoryInfo( baseFolder );
            foreach( FileInfo file in directory.GetFiles( "*.spritesheet" ) )
            {
                try
                {
                    Material mat = Serializable<Material>.Load( file.FullName );
                    if( mat != null )
                        materials[mat.Name] = mat;
                }
                catch { }        
            }

            this.Content = content;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {            
            base.Initialize();
        }

        public Material CreateMaterial( string name )
        {
            return Material.Copy( materials[name] );
        }

        public void Add( Material mat )
        {
            mat.Initialize( Content );
            this.materialList.Add( mat );
        }

        public Material Add( string mat )
        {
            Material material = CreateMaterial( mat );
            Add( material );
            return material;
        }

        public void Delete( Material mat )
        {
            this.materialDeleteList.Add( mat );
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update( GameTime gameTime )
        {
            foreach( Material mat in materialDeleteList )
            {
                materialList.Remove( mat );
            }

            foreach( Material mat in materialList )
            {
                mat.Update( gameTime );
            }

            base.Update( gameTime );
        }
    }
}