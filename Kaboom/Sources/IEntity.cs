using System.Collections.Generic;
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
    /// Define a drawable Kaboom component
    /// </summary>
    interface IEntity
    {
        /// <summary>
        /// SpriteSheet associated with the entity
        /// </summary>
        SpriteSheet Tile { get; set; }

        /// <summary>
        /// Mark the entity for destruction
        /// Allow it to play death animation before being removed
        /// </summary>
        bool MarkedForDestruction { get; set; }

        /// <summary>
        /// Position on Z-Axis
        /// Indice of superposition
        /// </summary>
        int ZIndex { get; set; }

        /// <summary>
        /// Define if the 
        /// </summary>
        EVisibility Visibility { get; set; }

        /// <summary>
        /// Draw an entity on the screen
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw textures</param>
        /// <param name="t">Game clock</param>
        /// <param name="r">Position position offset used to draw objects</param>
        void Draw(SpriteBatch sb, GameTime t, Point r);

        /// <summary>
        /// Update entity and entity's spritesheet
        /// </summary>
        /// <param name="time">game clock</param>
        void Update(GameTime time);
    }

    /// <summary>
    /// Comparer for SortedSet
    /// </summary>
    class EntityComparer : IComparer<IEntity>
    {
        /// <summary>
        /// Compare two entities by matching their Z-index
        /// </summary>
        /// <param name="a">First entity</param>
        /// <param name="b">Second entity</param>
        /// <returns></returns>
        public int Compare(IEntity a, IEntity b)
        {
            return a.ZIndex - b.ZIndex;
        }
    }
}