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

    #region Unitest
    /// <summary>
    /// Exemple of implementation
    /// </summary>
    class UnitestEntity : IEntity
    {
        private readonly SpriteSheet tile_;

        public UnitestEntity(int z, SpriteSheet tile, EVisibility v = EVisibility.Opaque)
        {
            this.ZIndex = z;
            this.Visibility = v;
            this.tile_ = tile;
        }

        public EVisibility Visibility { get; set; }

        public int ZIndex { get; set; }

        public void Update(GameTime time)
        {
            this.tile_.Update(time);
        }

        public void Draw(SpriteBatch sb, GameTime t, Point p)
        {
            // TODO: Do not draw outside screen.
            this.tile_.Draw(sb, t, p);
            //sb.Draw(this.tile_,
                //new Rectangle(
                //    (r.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                //    (r.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                //    Camera.Instance.DimX,
                //    Camera.Instance.DimY), Color.White);
        }
    }
    #endregion
}