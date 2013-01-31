using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    /// <summary>
    /// Definition of an animated Sprite
    /// </summary>
    class SpriteSheet
    {
        private readonly Texture2D spriteSheet_;
        private Rectangle frameSize_;
        private readonly double frameSpeed_;
        private int currentAnimation_;
        private int currentFrame_;
        private readonly int[] totalFrames_;
        private readonly int totalAnimations_;
        private double currentElapsedTime_;

        /// <summary>
        /// Constructor
        /// <param name="text"> Texture Loaded from the Library</param>
        /// <param name="framesPerAnim"> the number of frames each animations contains</param>
        /// <param name="animations"> the number of animations the sheet contains</param>
        /// <param name="frameSpeed"> the speed of rendering of the animation</param>
        /// </summary>
        public SpriteSheet(Texture2D text, int[] framesPerAnim, int animations = 1, double frameSpeed = 30.0)
        {
            this.spriteSheet_ = text;
            this.frameSpeed_ = frameSpeed;
            this.currentAnimation_ = 0;
            this.currentFrame_ = 0;
            this.totalFrames_ = framesPerAnim;
            this.totalAnimations_ = animations;
            this.currentElapsedTime_ = 0;
            this.frameSize_ = new Rectangle(0, 0, this.spriteSheet_.Width / this.totalFrames_.Max(),
                                            this.spriteSheet_.Height / this.totalAnimations_);
        }

        /// <summary>
        /// Update the current SpriteSheet and its animations.
        /// There, it just play the animations repeatedly.
        /// <param name="gameTime">Current Game Timer</param> 
        /// </summary>
        public void Update(GameTime gameTime)
        {
            this.currentElapsedTime_ += gameTime.ElapsedGameTime.TotalSeconds;

            if (this.currentElapsedTime_ >= 1 / this.frameSpeed_)
            {
                this.currentElapsedTime_ = 0.0;
                ++this.currentFrame_;
            }

            if (this.currentFrame_ >= this.totalFrames_[this.currentAnimation_])
            {
                this.currentFrame_ = 0;
                ++this.currentAnimation_;
                if (this.currentAnimation_ >= this.totalAnimations_)
                    this.currentAnimation_ = 0;
            }
        }

        /// <summary>
        /// Draw the current sprite of the animation on screen
        /// <param name="sb">SpriteBatch used to draw textures</param>   
        /// </summary>
        public void Draw(SpriteBatch sb, GameTime t, Point r)
        {
            sb.Draw(
                this.spriteSheet_,
                new Rectangle(
                    (r.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                    (r.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                    Camera.Instance.DimX,
                    Camera.Instance.DimY),
                new Rectangle(
                    this.frameSize_.Width * this.currentFrame_,
                    this.frameSize_.Height * this.currentAnimation_,
                    this.frameSize_.Width,
                    this.frameSize_.Height),
                Color.White);
        }
    };
}