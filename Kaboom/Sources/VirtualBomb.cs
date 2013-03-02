namespace Kaboom.Sources
{
    /// <summary>
    /// Temporary bomb used to see pattern
    /// </summary>
    class VirtualBomb : Bomb
    {
        private readonly Pattern.Type type_;
        private readonly SpriteSheet tile_;

        /// <summary>
        /// Initilize a virtual bomb
        /// </summary>
        /// <param name="type">Pattern</param>
        /// <param name="tile">Sprite</param>
        public VirtualBomb(Pattern.Type type, SpriteSheet tile)
            : base(type, tile, "highlight2")
        {
            type_ = type;
            tile_ = tile;
        }

        /// <summary>
        /// Create a new bomb based on attributes
        /// </summary>
        /// <returns>new bomb</returns>
        public Bomb ToBomb()
        {
            return new Bomb(type_, tile_, "highlight", this.GetPatternOrientation());
        }
    }
}
