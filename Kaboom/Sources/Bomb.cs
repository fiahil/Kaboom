using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    class Bomb : StaticEntity, IBomb
    {
        private readonly Pattern pattern_;
        private double timeBeforeExplosion_;
        private bool readyToExplode_;
        private bool animationLaunched_;

        public Bomb(Pattern.Type type, int zIndex, SpriteSheet tile) :
            base (zIndex, tile, EVisibility.Transparent)
        {
            pattern_ = new Pattern(type);
            readyToExplode_ = false;
            timeBeforeExplosion_ = 0;
            animationLaunched_ = false;
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
    }
}