
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
        public Block(SpriteSheet tile, bool isDestroyable)
            : base(5, tile, EVisibility.Opaque)
        {
            this.Destroyable = isDestroyable;
        }

        /// <summary>
        /// Define whether the entity is destroyable or not
        /// </summary>
        public bool Destroyable { get; set; }
    }
}