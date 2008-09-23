using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace Shoota.Materials
{
    /// <summary>
    /// A serialiazable Key/Value class.
    /// </summary>
    public class KeyValueEntry
    {
        public KeyValueEntry()
        {
        }

        public KeyValueEntry( string key, int[] value )
        {
            this.Key = key;
            this.Value = value;
        }

        public int this[int key]
        {
            get
            {
                return Value[key];
            }
            set
            {
                Value[key] = value;
            }
        }

        public string Key;
        public int[] Value;        
    }

    /// <summary>
    /// A container for key/value pairs.
    /// </summary>
    public class KeyValueList : List<KeyValueEntry>
    {
        private KeyValueEntry getKey( string key )
        {
            KeyValueEntry[] list = base.ToArray();
            foreach( KeyValueEntry t in list )
            {
                if( t.Key == key )
                    return t;
            }

            return null;
        }

        private void SetKeyValue( string key, int[] value )
        {
            KeyValueEntry[] list = base.ToArray();
            foreach( KeyValueEntry t in list )
            {
                if( t.Key == key )
                {
                    t.Value = value;
                    break;
                }
            }
        }

        public bool ContainsKey( string key )
        {
            return getKey( key ) != null;
        }

        new public void Add( KeyValueEntry item )
        {
            base.Add( item );
        }

        new public int Count()
        {
            return base.Count;
        }
 
        public KeyValueEntry this[string index]
        {
            get
            {
                return getKey( index );
            }
            protected set { }
        }

        new public KeyValueEntry this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
            }
        }
    }

    public enum LoopType
    {
        None,
        Repeat,
        Custom
    }

    public class Material : Serializable<Material>
    {

        #region Fields

        // The name of the material ( must be unique ).
        public string Name;

        // The colour, texture and pixel shader to use for this material.
        public Color Color;

        public string TextureName;
        public string EffectName;

        // A array of rectangles that define the individual frames from the spritesheet.
        public Rectangle[] Cells;

        // The total number of frames in the sprite sheet.
        public int TotalFrames;

        // A lookup table for animations, each animation is defined by a string name and two numbers, the start and end frames.
        public KeyValueList AnimLookupTable; // or a class derived from List which constains an implementation of ContainsKey

        // The speed at which animation will occur in Frames Per Second.
        public float Fps;

        public event EventHandler AnimationFinished;

        #endregion

        #region Properties

        [XmlIgnore]
        public Texture2D Texture
        {
            get;
            protected set;
        }

        [XmlIgnore]
        public Effect Effect
        {
            get;
            protected set;
        }

        [XmlIgnore]
        public Rectangle CurrentFrame
        {
            get;
            protected set;
        }

        [XmlIgnore]
        public int CurrentFrameIndex
        {
            get;
            protected set;
        }

        [XmlIgnore]
        public string CurrentAnim
        {
            get;
            protected set;
        }

        [XmlIgnore]
        public double ElapsedTime
        {
            get;
            protected set;
        }

        [XmlIgnore]
        public LoopType LoopType
        {
            get;
            set;
        }

        [XmlIgnore]
        public bool Enabled
        {
            get;
            set;
        }

        [XmlIgnore]
        public bool Initialized
        {
            get;
            protected set;
        }

        #endregion

        #region Methods

        public Material()
        {
            this.AnimLookupTable = new KeyValueList();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of material - Unique</param>
        /// <param name="effect">Pixel shader effect for material</param>
        public Material( string name, string effect, string tex, Color col, float fps )
        {
            this.Name = name;
            this.EffectName = effect;
            this.TextureName = tex;
            this.Fps = fps;
            this.Color = col;

            this.AnimLookupTable = new KeyValueList();
        }

        //public Material( Material mat )
        //{
         //   this.Name = mat.Name;
          //  this.EffectName = mat.EffectName;
           // this.TextureName = mat.TextureName;
           // this.Fps = mat.Fps;
            //this.Color = mat.Color;

         //   this.
        //
        //    this.AnimLookupTable = new KeyValueList();
        //}

        public void Initialize( ContentManager content )
        {
            if( this.Initialized )
                return;

            try
            {
                this.Texture = content.Load<Texture2D>( this.TextureName );
            }
            catch 
            { 
                this.Initialized = false;
                this.Enabled = false;
                throw new Exception( "Could'nt load texture!" );
            }

            try
            {
                this.Effect = content.Load<Effect>( this.EffectName );
            }
            catch { }

            this.Enabled = true;
            this.Initialized = true;
        }

        public static Material Copy( Material mat )
        {
            return (Material)mat.MemberwiseClone();
        }

        /// <summary>
        /// Creates a sprite sheet based on a grid of rectangles of even size taking up the entire texture size.
        /// </summary>
        /// <param name="rows">Number of rows in the sheet.</param>
        /// <param name="columns">Number of columns in the sheet.</param>
        public void CreateSpriteSheets( int rows, int columns )
        {
            this.TotalFrames = rows * columns;
            this.Cells = new Rectangle[this.TotalFrames];

            float width = this.Texture.Width / columns;
            float height = this.Texture.Height / rows;

            for( int i = 0; i < rows; i++ )
            {
                for( int j = 0; j < columns; j++ )
                {
                    this.Cells[( i * columns ) + j] = new Rectangle( (int)( j * width ), (int)( i * height ), (int)width, (int)height );
                }
            }
        }

        /// <summary>
        /// Adds a animation definition, these allow the game to play a specific animation by name.
        /// </summary>
        /// <param name="name">The name used to lookup the animation</param>
        /// <param name="startFrame">The starting frame for the animation.</param>
        /// <param name="endFrame">The ending frame for the animation.</param>
        public void AddAnimationDefinition( string ID, int startFrame, int endFrame )
        {
            if( startFrame < 0 )
                startFrame = 0;

            if( endFrame > TotalFrames )
                endFrame = TotalFrames;

            this.AnimLookupTable.Add( new KeyValueEntry( ID, new int[] { startFrame, endFrame } ) );
        }

        /// <summary>
        /// Sets an animation for the material.
        /// </summary>
        /// <param name="ID">The name of the animation to play.</param>
        public void SetAnimation( string ID )
        {
            KeyValueEntry anim = AnimLookupTable[ID];
            if( anim == null )
                return;

            this.CurrentAnim = ID;
            this.CurrentFrameIndex = anim[0];
            this.ElapsedTime = 0;

            this.Enabled = true;
        }

        /// <summary>
        /// Allows the material to update the current frame to render if nessary.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update( GameTime gameTime )
        {
            if( !Enabled )
                return;

            if( !AnimLookupTable.ContainsKey( this.CurrentAnim ) )
                return;

            int startFrame = AnimLookupTable[CurrentAnim][0];
            int endFrame = AnimLookupTable[CurrentAnim][1];

            this.ElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            int frames = endFrame - startFrame;

            if( this.ElapsedTime > ( 1 / this.Fps ) )
            {
                this.CurrentFrameIndex++;
                this.ElapsedTime = 0;
            }

            if( this.CurrentFrameIndex > endFrame )
            {
                //this.currentFrameIndex = startFrame;
                switch( this.LoopType )
                {
                    case LoopType.None:
                        this.CurrentFrameIndex -= 1;
                        this.Enabled = false;
                        break;

                    case LoopType.Repeat:
                        this.CurrentFrameIndex = startFrame;
                        break;

                    case LoopType.Custom:
                        if( AnimationFinished != null )
                            AnimationFinished( this, null );
                        break;

                    default:
                        this.CurrentFrameIndex = startFrame;
                        break;
                }
            }

            this.CurrentFrame = this.Cells[this.CurrentFrameIndex];
        }

        #endregion
    }
}