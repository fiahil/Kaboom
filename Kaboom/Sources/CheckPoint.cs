namespace Kaboom.Sources
{
    class CheckPoint : Entity
    {
        private bool activated_;

        /// <summary>
        /// Creates a new CheckPoint
        /// </summary>
        /// <param name="tile">Spritesheet of the checkpoint</param>
        /// <param name="time">Time before explosion</param>
        /// <param name="activated">Activate checkpoint</param>
        /// <param name="bombsetidx"></param>
        public CheckPoint(SpriteSheet tile, int time, bool activated, int bombsetidx) :
            base(2, tile)
        {
            Time = time;
            Activated = activated;
            BombsetIdx = bombsetidx;
        }

        public bool Activated
        {
            get { return activated_; }
            set
            {
                activated_ = value;
                this.Tile.CAnimation = value ? 1 : 0;
            }
        }

        public int BombsetIdx { get; set; }
        public int Time { get; private set; }
    }
}
