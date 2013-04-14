
using Microsoft.Xna.Framework;

namespace Kaboom.Sources
{
    class Explosable : Entity
    {
        private double timeBeforeExplosion_;
        private bool readyToExplode_;
        private bool animationLaunched_;

        public Explosable(int zindex, SpriteSheet tile, EVisibility visibility = EVisibility.Opaque)
            : base(zindex, tile, visibility)
        {
            readyToExplode_ = false;
            timeBeforeExplosion_ = 0;
            animationLaunched_ = false;
            MarkedForDestruction = false;
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
        /// Specifies whether the explosable component must explode or not
        /// </summary>
        /// <returns></returns>
        public virtual bool IsReadyToExplode()
        {
            if (!(readyToExplode_ && timeBeforeExplosion_ <= 0) && !animationLaunched_)
                return false;
            if (!animationLaunched_)
                Tile.CAnimation = 1;
            animationLaunched_ = true;
            return true;
        }

        /// <summary>
        /// Start the timer before the explosable component explodes
        /// </summary>
        /// <param name="time">Time to wait before explosion (in milliseconds)</param>
        public bool SetForExplosion(double time)
        {
            if (readyToExplode_)
                return false;
            readyToExplode_ = true;
            timeBeforeExplosion_ = time;
            return true;
        }

        public void ResetExplosionSettings()
        {
            readyToExplode_ = false;
            timeBeforeExplosion_ = 0;
            animationLaunched_ = false;
        }

        /// <summary>
        /// Indicate whether the explosable component will be removed
        /// </summary>
        public bool MarkedForDestruction { get; set; }
    }
}
