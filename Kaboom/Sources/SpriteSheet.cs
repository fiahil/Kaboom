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
            private readonly Point head_;
            private readonly int nbTotalFrames_;
            private readonly bool isCycle_;

            public Anim(Point h, int frames, int lines, double frameSpeed, bool isCycle = false)
            {
                this.head_ = h;
                this.nbTotalFrames_ = frames;
                this.NbLines = lines;
                this.Speed = frameSpeed;
                this.isCycle_ = isCycle;
            }

            public double Speed { get; private set; }

            public bool Cycle
            {
                get { return this.isCycle_; }
            }

            public int Totalframes
            {
                get { return this.nbTotalFrames_; }
            }

            public Point Head
            {
                get { return this.head_; }
            }
// ReSharper disable UnusedAutoPropertyAccessor.Local
            private int NbLines { get; set; }
// ReSharper restore UnusedAutoPropertyAccessor.Local
        }

        private readonly Texture2D spriteSheet_;
        private Rectangle frameSize_;
        private readonly Dictionary<int, Anim> anims_;
        private readonly Event event_;
        private int currentAnimation_;
        private int currentFrame_;
        private int currentLine_;
        private double currentElapsedTime_;
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
            this.spriteSheet_ = text;
            this.currentAnimation_ = 0;
            this.currentFrame_ = 0;
            this.currentLine_ = 0;
            this.currentElapsedTime_ = 0;
            this.event_ = new Event();

            this.anims_ = new Dictionary<int, Anim>(); // int useless, a terme mettre un enum qui fit bien.

            this.frameSize_ = new Rectangle(0, 0, this.spriteSheet_.Width / framesPerAnim.Max(),
                                        this.spriteSheet_.Height / animations);
 
            this.anims_.Add(0, new Anim(new Point(0, 0), framesPerAnim[0], 1, frameSpeed, true));
            for (var i = 1; i < animations; ++i)
            {
                this.anims_.Add(i, new Anim(new Point(0, i), framesPerAnim[i], 1, frameSpeed));
            }
        }

        ///  <summary>
        /// 
        ///  </summary>
        /// <param name="text"> The SpriteSheet</param>
        /// <param name="framesPerAnim">The number of Frames of each animations</param>
        /// <param name="linesPerAnim">The number of Lines for each animations</param>
        /// <param name="frameSpeed">the average speed given to the animations</param>
        /// <param name="cycles"></param>
        /// <param name="animations">the number of animation on the SpriteSheet</param>
        public SpriteSheet(Texture2D text, int[] framesPerAnim, int[] linesPerAnim, double[] frameSpeed, bool[] cycles, int animations = 1)
        {
            this.spriteSheet_ = text;
            this.currentAnimation_ = 0;
            this.currentFrame_ = 0;
            this.currentElapsedTime_ = 0;
            this.event_ = new Event();

            this.anims_ = new Dictionary<int, Anim>(); // int useless, a terme mettre un enum qui fit bien.

            this.frameSize_ = new Rectangle(0, 0, this.spriteSheet_.Width / framesPerAnim.Max(),
                                                this.spriteSheet_.Height /animations);

            for (var i = 0; i < animations; ++i)
            {
                this.anims_.Add(i, new Anim(new Point(0, i), framesPerAnim[i], linesPerAnim[i], frameSpeed[i], cycles[i]));
                i += linesPerAnim[i] - 1;
            }
        }

        /// <summary>
        /// Update the current SpriteSheet and its animations.
        /// There, it just play the animations repeatedly.
        /// <param name="gameTime">Current Game Timer</param> 
        /// </summary>
        public void Update(GameTime gameTime)
        {
            this.currentElapsedTime_ += gameTime.ElapsedGameTime.TotalSeconds;

            if (this.currentElapsedTime_ >= 1 / this.anims_[this.currentAnimation_].Speed)
            {
                this.currentElapsedTime_ = 0.0;
                ++this.currentFrame_;
                if (this.currentFrame_ * this.frameSize_.Width >= this.spriteSheet_.Width)
                    ++this.currentLine_;
            }

            if (this.currentFrame_ < this.anims_[this.currentAnimation_].Totalframes) return;
            if (this.anims_[this.currentAnimation_].Cycle == false)
            {
                if (AnimationDone != null)
                    AnimationDone(this, null);
            }
            this.currentFrame_ = 0;
            this.currentLine_ = 0;
        }

        /// <summary>
        /// Draw the current sprite of the animation on screen
        /// <param name="sb">SpriteBatch used to draw textures</param>   
        /// </summary>
        public void Draw(SpriteBatch sb, GameTime t, Point p)
        {
            sb.Draw(
                this.spriteSheet_,
                new Rectangle(
                    (p.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                    (p.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                    Camera.Instance.DimX,
                    Camera.Instance.DimY),
                new Rectangle(
                    this.frameSize_.Width * this.currentFrame_,
                    this.frameSize_.Height * (this.anims_[this.currentAnimation_].Head.Y + this.currentLine_),
                    this.frameSize_.Width,
                    this.frameSize_.Height),
                Color.White);
        }
        /// <summary>
        /// Draw the current sprite of the animation on screen
        /// <param name="sb">SpriteBatch used to draw textures</param>   
        /// </summary>
        public void Draw(SpriteBatch sb, GameTime t, Rectangle r)
        {
            sb.Draw(
                this.spriteSheet_,
                r,
                new Rectangle(
                    this.frameSize_.Width * this.currentFrame_,
                    this.frameSize_.Height * (this.anims_[this.currentAnimation_].Head.Y + this.currentLine_),
                    this.frameSize_.Width,
                    this.frameSize_.Height),
                Color.White);
        }
        public void ResetCurrentAnim()
        {
            this.currentFrame_ = 0;
        }
        public double Speed
        {
            get { return this.anims_[this.currentAnimation_].Speed; }
        }
        public int CAnimation
        {
            get { return this.currentAnimation_; }
            set { this.currentAnimation_ = value; }
        }

        public Event Event
        {
            get { return event_; }
        }
    };
                   
 
    /// <summary>
    /// Definition of a Font who represent a Latin Style alphabet (26char)
    /// plus the numerics and several special char.
    /// </summary>
    class TtFont
    {
        private readonly SpriteFont   font_;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="font">the loaded font who represent the current used alphabet</param>
        public TtFont(SpriteFont font)
        {
            this.font_ = font;
        }

        /// <summary>
        /// Method who allow the user to display a string on the screen at the position he wants
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw textures</param>
        /// <param name="str">the string to display on screen</param>
        /// <param name="posInScreen">the position where the display ll be printed</param>
        public void PutstrOnScreen(SpriteBatch sb, string str, Rectangle posInScreen)
        {
            sb.Begin();
            sb.DrawString(this.font_, "<- Mets ton message ici ->", new Vector2(posInScreen.X, posInScreen.Y), Color.White);
            sb.End();
        }
    };
}