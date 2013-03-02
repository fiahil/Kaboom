using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    /// <summary>
    /// Definition of an animated Sprite
    /// </summary>
    internal class SpriteSheet : ICloneable
    {
        /// <summary>
        /// Represent the animations of the SpriteSheet
        /// </summary>
        private class Anim
        {
            private readonly Point head_;
            private readonly int nbTotalFrames_;
            private readonly bool isCycle_;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="h">A point who represent the beginning of the animation</param>
            /// <param name="frames">the number of frames of the animation</param>
            /// <param name="frameSpeed">the speed of the animation</param>
            /// <param name="isCycle">define if the animation have to be done in cycle or only one time</param>
            /// 
            public Anim(Point h, int frames, double frameSpeed, bool isCycle = false)
            {
                this.head_ = h;
                this.nbTotalFrames_ = frames;
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
        }

        #region Attributes
        private readonly Texture2D spriteSheet_;
        private Rectangle frameSize_;
        private readonly Dictionary<int, Anim> anims_;
        private readonly Event event_;
        private int currentAnimation_;
        private int currentFrame_;
        private int currentLine_;
        private double currentElapsedTime_;
        public event EventHandler AnimationDone;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">The SpriteSheet</param>
        /// <param name="framesPerAnim">The number of Frames of each animations</param>
        /// <param name="animations">the number of animation on the SpriteSheet</param>
        /// <param name="frameSpeed">the average speed given to the animations</param>
        public SpriteSheet(Texture2D text, int[] framesPerAnim, int animations, double frameSpeed = 30.0)
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

            this.anims_.Add(0, new Anim(new Point(0, 0), framesPerAnim[0], frameSpeed, true));
            for (var i = 1; i < animations; ++i)
            {
                this.anims_.Add(i, new Anim(new Point(0, i), framesPerAnim[i], frameSpeed));
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
        public SpriteSheet(Texture2D text, int[] framesPerAnim, int[] linesPerAnim, double[] frameSpeed, bool[] cycles,
                           int animations)
        {
            this.spriteSheet_ = text;
            this.currentAnimation_ = 0;
            this.currentFrame_ = 0;
            this.currentElapsedTime_ = 0;
            this.event_ = new Event();

            this.anims_ = new Dictionary<int, Anim>(); // int useless, a terme mettre un enum qui fit bien.

            this.frameSize_ = new Rectangle(0, 0, this.spriteSheet_.Width / framesPerAnim.Max(),
                                            this.spriteSheet_.Height / animations);

            for (var i = 0; i < animations; ++i)
            {
                this.anims_.Add(i, new Anim(new Point(0, i), framesPerAnim[i], frameSpeed[i], cycles[i]));
                i += linesPerAnim[i] - 1;
            }
        }
        /// <summary>
        /// Constructor used by the clone methods.
        /// Create a new SpriteSheet who is exactly the same
        /// than the one taken in parameters.
        /// </summary>
        /// <param name="ss">The spriteSheet to clone</param>
        private SpriteSheet(SpriteSheet ss)
        {
            this.spriteSheet_ = ss.spriteSheet_;
            this.frameSize_ = ss.frameSize_;
            this.anims_ = ss.anims_;
            this.event_ = ss.event_;
            this.currentAnimation_ = ss.currentAnimation_;
            this.AnimationDone = ss.AnimationDone;
            this.currentFrame_ = ss.currentFrame_;              // TODO : Réinitialiser à 0?
            this.currentLine_ = ss.currentLine_;                // TODO : Réinitialiser à 0?
            this.currentElapsedTime_ = ss.currentElapsedTime_;  // TODO : Réinitialiser à 0?
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
        public void Draw(SpriteBatch sb, GameTime t, Point p, int layerDepth)
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
                Color.White,0, new Vector2(0,0),0, depth: layerDepth);
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

        /// <summary>
        /// Reset the current animation
        /// </summary>
        public void ResetCurrentAnim()
        {
            this.currentFrame_ = 0;
        }

        /// <summary>
        /// Clone the current spritesheet into a new object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new SpriteSheet(this);
        }

        #region Get Set
        public double Speed
        {
            get { return this.anims_[this.currentAnimation_].Speed; }
        }

        public int CAnimation
        {
            get
            {
                return this.currentAnimation_;
            }
            set
            {
                if (value >= this.anims_.Count || value < 0) return;
                this.currentAnimation_ = value;
                this.currentFrame_ = 0;
            }
        }

        public Event Event
        {
            get { return event_; }
        }
        #endregion
    };
}