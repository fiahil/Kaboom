using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    /// <summary>
    /// Define Entities' visibility
    /// </summary>
    enum EVisibility
    {
        Opaque,
        Transparent
    }

    /// <summary>
    /// Basic entity
    /// </summary>
    class Entity
    {
        /// <summary>
        /// Construct a new entity
        /// </summary>
        /// <param name="zindex">Z-Axis' indice of supperposition</param>
        /// <param name="tile">Texture associated with the entity</param>
        /// <param name="visibility">Entity's visibility</param>
        public Entity(int zindex, SpriteSheet tile, EVisibility visibility = EVisibility.Opaque)
        {
            this.ZIndex = zindex;
            this.Visibility = visibility;
            this.Tile = tile;
        }

        /// <summary>
        /// SpriteSheet associated with the entity
        /// </summary>
        public SpriteSheet Tile { get; set; }

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
        /// <param name="depth">the depth on the screen of the sprite</param>
        public virtual void Draw(SpriteBatch sb, GameTime t, Point p, int depth)
        {
            this.Tile.Draw(sb, t, p, depth);
        }
    }
}