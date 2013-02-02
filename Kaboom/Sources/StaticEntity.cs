using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    /// <summary>
    /// Exemple of implementation
    /// </summary>
    class StaticEntity : IEntity
    {
        private readonly SpriteSheet tile_;

        /// <summary>
        /// Construct a new entity
        /// </summary>
        /// <param name="zindex">Z-Axis' indice of supperposition</param>
        /// <param name="tile">Texture associated with the entity</param>
        /// <param name="visibility">Entity's visibility</param>
        public StaticEntity(int zindex, SpriteSheet tile, EVisibility visibility = EVisibility.Opaque)
        {
            this.ZIndex = zindex;
            this.Visibility = visibility;
            this.tile_ = tile;
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
            this.tile_.Update(time);
        }

        /// <summary>
        /// Draw sprite
        /// </summary>
        /// <param name="sb">spritebatch used to render texture</param>
        /// <param name="t">game clock</param>
        /// <param name="p">Coordinates of the square containing the entity</param>
        public void Draw(SpriteBatch sb, GameTime t, Point p)
        {
            this.tile_.Draw(sb, t, p);
        }
    }
}