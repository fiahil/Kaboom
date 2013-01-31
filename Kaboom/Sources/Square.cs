using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    /// <summary>
    /// Storage class for entities.
    /// Used a SortedSet to sort entities using their Z-Index
    /// </summary>
    class Square
    {
        private readonly SortedSet<IEntity> entities_;
        private readonly Point base_;

        /// <summary>
        /// Square ctor
        /// </summary>
        /// <param name="baseLoc">Map coordinates</param>
        public Square(Point baseLoc)
        {
            this.entities_ = new SortedSet<IEntity>(new EntityComparer());
            this.base_ = baseLoc;
        }

        /// <summary>
        /// Add an entity to the square
        /// </summary>
        /// <param name="e">The entity to add</param>
        public void AddEntity(IEntity e)
        {
            this.entities_.Add(e);
        }

        /// <summary>
        /// Call entities' update
        /// </summary>
        public void Update(GameTime time)
        {
            foreach (var entity in this.entities_)
            {
                entity.Update(time);
            }
        }

        /// <summary>
        /// Get Highest Z-Index form SortedSet
        /// </summary>
        /// <returns>Z-Index</returns>
        public int GetMaxZIndex()
        {
            return this.entities_.Max.ZIndex;
        }

        /// <summary>
        /// Draw entities using SpriteBatch. Should be called between sb.begin() & sb.end()
        /// </summary>
        /// <param name="sb">SpriteBatch used to render testures</param>
        /// <param name="t">Game clock used for Sprites' animation</param>
        public void Draw(SpriteBatch sb, GameTime t)
        {
            var opaqueCount = 0;
            foreach (var item in this.entities_)
            {
                if (item.Visibility == EVisibility.Opaque)
                    opaqueCount++;
                if (opaqueCount > 1)
                     break;
                item.Draw(sb, t, this.base_);
             }
        }

        /// <summary>
        /// Getter for base coordinates
        /// </summary>
        public Point Base
        {
            get { return this.base_; }
        }

        #region Unitest
        /// <summary>
        /// Square Unitests
        /// </summary>
        public static void Unitest()
        {
            var sq = new Square(Point.Zero);

            // Z-index test
            sq.AddEntity(new UnitestEntity(0, null));
            sq.AddEntity(new UnitestEntity(2, null));
            sq.AddEntity(new UnitestEntity(5, null, EVisibility.Transparent));
            sq.AddEntity(new UnitestEntity(1, null));
            sq.AddEntity(new UnitestEntity(2, null));

            // Put your breakpoint here
        }
        #endregion
    }
}
