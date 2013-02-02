using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Kaboom.Sources
{

   
    /// <summary>
    /// Definition of an animated Sprite
    /// </summary>
    class SpriteSheet
    {
        class Anim
        {
            private Point _head;
            private int _nbTotalFrames;
            private int _nbLines;
            private double _speed;
            private bool _isCycle;

            public Anim(Point h, int frames, int lines, double frameSpeed, bool isCycle = false)
            {
                this._head = h;
                this._nbTotalFrames = frames;
                this._nbLines = lines;
                this._speed = frameSpeed;
                this._isCycle = isCycle;
            }

            public double Speed
            {
                get { return this._speed; }
                set { this._speed = value; }
            }
            public bool Cycle
            {
                get { return this._isCycle; }
            }
            public Point head
            {
                get { return this._head; }
            }
            public int Totalframes
            {
                get { return this._nbTotalFrames; }
            }
        }

        private Texture2D _SpriteSheet;
        private Rectangle _frameSize;
        private Dictionary<int, Anim> _anims;
        private readonly Event _event;
        private int _currentAnimation;
        private int _currentFrame;
        private double _currentElapsedTime;
        public event EventHandler AnimationDone;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">The SpriteSheet</param>
        /// <param name="framesPerAnim">The number of Frames of each animations</param>
        /// <param name="animations">the number of animation on the SpriteSheet</param>
        /// <param name="frameSpeed">the average speed given to the animations</param>
        public SpriteSheet(Texture2D text, int[] framesPerAnim, int animations = 1, double frameSpeed = 30.0)
        {
            this._SpriteSheet = text;
            this._currentAnimation = 0;
            this._currentFrame = 0;
            this._currentElapsedTime = 0;
            this._event = new Event();

            this._anims = new Dictionary<int, Anim>(); // int useless, a terme mettre un enum qui fit bien.

            this._frameSize = new Rectangle(0, 0, (int)(this._SpriteSheet.Width / framesPerAnim.Max()),
                                        (int)(this._SpriteSheet.Height / animations));
 
            this._anims.Add(0, new Anim(new Point(0, 0), framesPerAnim[0], 1, frameSpeed, true));
            for (int i = 1; i < animations; ++i)
            {
                this._anims.Add(i, new Anim(new Point(0, i * this._frameSize.Y), framesPerAnim[i], 1, frameSpeed));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"> The SpriteSheet</param>
        /// <param name="framesPerAnim">The number of Frames of each animations</param>
        /// <param name="linesPerAnim">The number of Lines for each animations</param>
        /// <param name="frameSpeed">the average speed given to the animations</param>
        /// <param name="animations">the number of animation on the SpriteSheet</param>
        public SpriteSheet(Texture2D text, int[] framesPerAnim, int[] linesPerAnim, double[] frameSpeed, int animations = 1)
        {
            this._SpriteSheet = text;
            this._currentAnimation = 0;
            this._currentFrame = 0;
            this._currentElapsedTime = 0;
            this._event = new Event();

            this._anims = new Dictionary<int, Anim>(); // int useless, a terme mettre un enum qui fit bien.

            this._frameSize = new Rectangle(0, 0, (int)(this._SpriteSheet.Width / framesPerAnim.Max()),
                                                (int)(this._SpriteSheet.Height /animations));

            this._anims.Add(0, new Anim(new Point(0, 0), framesPerAnim[0], linesPerAnim[0], frameSpeed[0], true));

            for (int i = 1; i < animations; ++i)
            {
                this._anims.Add(i, new Anim(new Point(0, i * this._frameSize.Height), framesPerAnim[i], linesPerAnim[i], frameSpeed[i]));
            }
        }

        /// <summary>
        /// Update the current SpriteSheet and its animations.
        /// There, it just play the animations repeatedly.
        /// <param name="gameTime">Current Game Timer</param> 
        /// </summary>
        public void Update(GameTime gameTime)
        {
         this._currentElapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

         if (this._currentElapsedTime >= 1 / this._anims[this._currentAnimation].Speed)
         {
             this._currentElapsedTime = 0.0;
             ++this._currentFrame;
         }

         if (this._currentFrame >= this._anims[this._currentAnimation].Totalframes)
         {
             //
             // Event
             //
             
             if (this._anims[this._currentAnimation].Cycle == false)
             {
                 if (AnimationDone != null)
                     AnimationDone(this, null); 
             }
             this._currentFrame = 0;
         }

        }

        /// <summary>
        /// Draw the current sprite of the animation on screen
        /// <param name="sb">SpriteBatch used to draw textures</param>   
        /// </summary>
        public void Draw(SpriteBatch sb, GameTime t, Point r)
        {
            sb.Draw(
                this._SpriteSheet,
                new Rectangle(
                    (r.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                    (r.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                    Camera.Instance.DimX,
                    Camera.Instance.DimY),
                new Rectangle(
                    this._frameSize.Width * this._currentFrame,
                    this._frameSize.Height * (int)(this._anims[this._currentAnimation].head.Y / this._frameSize.Height),
                    this._frameSize.Width,
                    this._frameSize.Height),
                Color.White);
        }
        public void ResetCurrentAnim()
        {
            this._currentFrame = 0;
        }
        public double Speed
        {
            get { return this._anims[this._currentAnimation].Speed; }
        }
        public int Animation
        {
            get { return this._currentAnimation; }
            set { this._currentAnimation = value; }
        }
   };
                   
 
    /// <summary>
    /// Definition of a Font who represent a Latin Style alphabet (26char)
    /// plus the numerics and several special char.
    /// </summary>
    class TTFont
    {
        private SpriteFont   _font;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="font">the loaded font who represent the current used alphabet</param>
        public TTFont(SpriteFont font)
        {
            this._font = font;
        }

        /// <summary>
        /// Method who allow the user to display a string on the screen at the position he wants
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw textures</param>
        /// <param name="str">the string to display on screen</param>
        /// <param name="posInScreen">the position where the display ll be printed</param>
        public void putstrOnScreen(SpriteBatch sb, string str, Rectangle posInScreen)
        {
            sb.Begin();
            sb.DrawString(this._font, "<- Mets ton message ici ->", new Vector2(posInScreen.X, posInScreen.Y), Color.White);
            sb.End();
        }
    };
}