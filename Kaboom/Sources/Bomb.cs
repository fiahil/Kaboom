using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Kaboom.Sources
{
    class Bomb : Entity
    {
        private readonly Pattern pattern_;
        private double timeBeforeExplosion_;
        private bool readyToExplode_;
        private bool animationLaunched_;

        /// <summary>
        /// Construct a new bomb on Z-index 10
        /// </summary>
        /// <param name="type">Pattern type</param>
        /// <param name="tile">Bomb Skin</param>
        public Bomb(Pattern.Type type, SpriteSheet tile) :
            base (10, tile, EVisibility.Transparent)
        {
            pattern_ = new Pattern(type);
            readyToExplode_ = false;
            timeBeforeExplosion_ = 0;
            animationLaunched_ = false;
            this.MarkedForDestruction = false;
        }

        /// <summary>
        /// Update spritesheet and waiting for explosion
        /// </summary>
        /// <param name="time"></param>
        public override void Update(GameTime time)
        {
            base.Update(time);

            if (readyToExplode_)
                timeBeforeExplosion_ -= time.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Specifies whether the bomb must explode or not
        /// </summary>
        /// <returns></returns>
        public bool IsReadyToExplode()
        {
            if (!(readyToExplode_ && timeBeforeExplosion_ <= 0))
                return false;
            if (!animationLaunched_)
                Tile.CAnimation = 1;
            animationLaunched_ = true;
            readyToExplode_ = false;
            return true;
        }

        /// <summary>
        /// Get pattern
        /// </summary>
        /// <returns>A list of points representing the explosion pattern</returns>
        public List<Point> GetPattern()
        {
            return pattern_.GetPattern();
        }

        /// <summary>
        /// Start the timer before the bomb explodes
        /// </summary>
        /// <param name="time">Time to wait before explosion (in milliseconds)</param>
        public void SetForExplosion(double time)
        {
            if (readyToExplode_)
                return;
            readyToExplode_ = true;
            timeBeforeExplosion_ = time;
        }

        /// <summary>
        /// Indicate whether the bomb will be removed
        /// </summary>
        public bool MarkedForDestruction { get; set; }
    }
}