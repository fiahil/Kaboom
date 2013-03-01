namespace Kaboom.Sources
{
    class CheckPoint : Entity
    {
        /// <summary>
        /// Creates a new CheckPoint
        /// </summary>
        /// <param name="tile">Spritesheet of the checkpoint</param>
        /// <param name="time">Time before explosion</param>
        public CheckPoint(SpriteSheet tile, int time) :
            base(2, tile)
        {
            Time = time;
        }

        public int Time { get; private set; }
    }
}
