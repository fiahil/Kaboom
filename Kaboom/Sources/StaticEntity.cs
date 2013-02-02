using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    /// <summary>
    /// Exemple of implementation
    /// </summary>
    class StaticEntity : IEntity
    {
        public SpriteSheet Tile;
        public bool MarkedForDestroy { get; set; }

        /// <summary>
        /// Construct a new entity
        /// </summary>
        /// <param name="zindex">Z-Axis' indice of supperposition</param>
        /// <param name="tile">Texture associated with the entity</param>
        /// <param name="visibility">Entity's visibility</param>
        public StaticEntity(int zindex, SpriteSheet tile, EVisibility visibility = EVisibility.Opaque)
        {
            MarkedForDestroy = false;
            this.ZIndex = zindex;
            this.Visibility = visibility;
            this.Tile = tile;
        }

        /// <summary>
        /// Entity's visibility
        /// </summary>
        public EVisibility Visibility { get; set; }

        /// <summary>
        /// Z-axis' indice of supperposition
        /// </summary>
        public int ZIndex { get; set; }

        /// <summary>
        /// Update spritesheet
        /// </summary>
        /// <param name="time"></param>
        public virtual void Update(GameTime time)
        {
            this.Tile.Update(time);
        }

        /// <summary>
        /// Draw sprite
        /// </summary>
        /// <param name="sb">spritebatch used to render texture</param>
        /// <param name="t">game clock</param>
        /// <param name="p">Coordinates of the square containing the entity</param>
        public void Draw(SpriteBatch sb, GameTime t, Point p)
        {
            this.Tile.Draw(sb, t, p);
        }
    }
}