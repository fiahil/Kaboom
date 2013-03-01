
namespace Kaboom.Sources
{
    /// <summary>
    /// Destroyable asset
    /// </summary>
    class Block : Explosable
    {
        /// <summary>
        /// Initialize a block component
        /// </summary>
        /// <param name="tile">Sprite</param>
        /// <param name="isDestroyable">Destructability of the entity</param>
        /// <param name="endBlock"></param>
        public Block(SpriteSheet tile, bool isDestroyable, bool endBlock = false)
            : base(4, tile, EVisibility.Opaque)
        {
            EndBlock = endBlock;
            this.Destroyable = isDestroyable;
        }

        /// <summary>
        /// Define whether the entity is destroyable or not
        /// </summary>
        public bool Destroyable { get; set; }
        public bool EndBlock { get; private set; }

    }
}