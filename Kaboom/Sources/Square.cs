using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    /// <summary>
    /// Storage class for entities.
    /// Used a table to sort entities using their Z-Index
    /// </summary>
    class Square
    {
        public delegate void ExplosionHandler(Bomb bomb, Point pos);

        private readonly Entity[] entities_;
        public event ExplosionHandler Explosion;

        /// <summary>
        /// Square ctor
        /// </summary>
        /// <param name="baseLoc">Map coordinates</param>
        public Square(Point baseLoc)
        {
            this.entities_ = new Entity[6];
            this.Base = baseLoc;
        }

        /// <summary>
        /// Try to merge the new bomb with the current bomb
        /// </summary>
        /// <param name="entity">Bomb to add</param>
        /// <returns>Merge succeded or not</returns>
        private bool MergeBombs(Entity entity)
        {
            return ((Bomb)this.entities_[3]).Merge((Bomb)entity);
        }

        public void RemoveEntity(int offset)
        {
            if (entities_[3] != null)
                entities_[3].Consistency = EConsistence.Real;
            if (offset < this.entities_.Length)
            this.entities_[offset] = null;
        }

        /// <summary>
        /// Add an entity to the square
        /// </summary>
        /// <param name="entity">The entity to add</param>
        public bool AddEntity(Entity entity)
        {
            entity.Tile.AnimationDone +=
                (sender, ea) =>
                    {
                        foreach (
                            var e in
                                this.entities_.Where(e => e != null && e.Tile == sender && e is Explosable).Select(
                                    e => e as Explosable))
                        {
                            e.MarkedForDestruction = true;
                        }
                    };

            if ((entity is Bomb) && this.entities_[4] != null)
                return false;

            if (entity is VirtualBomb)
            {
                if (this.entities_[3] != null)
                {
                    if (!((Bomb) entity).Merge((Bomb) entities_[3]))
                        return false;
                    entities_[3].Consistency = EConsistence.Virtual;
                }
                this.entities_[5] = entity;

                return true;
            }

            if (entity is Bomb && this.entities_[3] != null)
            {
                if (MergeBombs(entity))
                {
                    this.entities_[5] = null;
                    entities_[3].Consistency = EConsistence.Real;
                    return true;
                }
                return false;
            }

            if (this.entities_[entity.ZIndex] == null)
            {
                this.entities_[5] = null;
                this.entities_[entity.ZIndex] = entity;
                this.entities_[entity.ZIndex].Consistency = EConsistence.Real;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Call entities' update
        /// </summary>
        public void Update(GameTime time)
        {
            for (var i = 0; i < this.entities_.Length; i++)
            {
                if (this.entities_[i] == null) continue;

                this.entities_[i].Update(time);
                if (this.entities_[i] is Explosable)
                {
                    var explosable = this.entities_[i] as Explosable;
                    if (explosable.IsReadyToExplode() && Explosion != null)
                        Explosion(explosable as Bomb, this.Base);
                    if (explosable.MarkedForDestruction)
                        this.entities_[i] = null;
                }
            }
        }

        /// <summary>
        /// Get Highest Z-Index
        /// </summary>
        /// <returns>Z-Index</returns>
        public int GetMaxZIndex()
        {
            return this.entities_.Reverse().Where(elt => elt != null).Select(elt => elt.ZIndex).FirstOrDefault();
        }

        /// <summary>
        /// Draw entities using SpriteBatch. Should be called between sb.begin() & sb.end()
        /// </summary>
        /// <param name="sb">SpriteBatch used to render testures</param>
        /// <param name="t">Game clock used for Sprites' animation</param>
        public void Draw(SpriteBatch sb, GameTime t)
        {
            var opaqueAllowed = 0;
            foreach (var entity in this.entities_.Where(e => e != null).Reverse().TakeWhile(
                entity => entity.Visibility == EVisibility.Transparent ||
                          (entity.Visibility == EVisibility.Opaque && opaqueAllowed++ == 0)).Reverse().Where(entity => entity.Consistency == EConsistence.Real))
            {
                entity.Draw(sb, t, this.Base, 1);
            }
        }

        /// <summary>
        /// Getter for base coordinates
        /// </summary>
        public Point Base { get; private set; }

        public void ActiveDetonator()
        {
            if (this.entities_[2] != null)
                Explode(((CheckPoint)this.entities_[2]).Time);
        }

        /// <summary>
        /// Set explosion marker on all bombs and blocks in the square
        /// </summary>
        public void Explode(double time)
        {
            if (this.entities_[4] != null && ((Block)this.entities_[4]).Destroyable)
                ((Block)this.entities_[4]).SetForExplosion(time);
            if (this.entities_[3] != null)
                ((Bomb)this.entities_[3]).SetForExplosion(time);
        }

        #region Unitest
        /// <summary>
        /// Square Unitests
        /// </summary>
        public static void Unitest()
        {
            var sq = new Square(Point.Zero);

            // Z-index test
            sq.AddEntity(new Entity(0, null));
            sq.AddEntity(new Entity(2, null));
            sq.AddEntity(new Entity(5, null, EVisibility.Transparent));
            sq.AddEntity(new Entity(1, null));
            sq.AddEntity(new Entity(2, null));

            // Put your breakpoint here
        }
        #endregion
    }
}
