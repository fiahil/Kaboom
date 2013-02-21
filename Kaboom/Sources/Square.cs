using System.Collections.Generic;
using System.Linq;
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
        public delegate void ExplosionHandler(Bomb bomb, Point pos);

        private readonly SortedSet<Entity> entities_;
        private readonly Point base_;
        public event ExplosionHandler Explosion;

        /// <summary>
        /// Square ctor
        /// </summary>
        /// <param name="baseLoc">Map coordinates</param>
        public Square(Point baseLoc)
        {
            this.entities_ = new SortedSet<Entity>(new EntityComparer());
            this.base_ = baseLoc;
        }

        /// <summary>
        /// Add an entity to the square
        /// </summary>
        /// <param name="entity">The entity to add</param>
        public void AddEntity(Entity entity)
        {
            entity.Tile.AnimationDone += 
                (sender, ea) =>
                    {
                        foreach (var e in this.entities_.Where(e => e.Tile == sender && e is Explosable).Select(e => e as Explosable))
                        {
                            e.MarkedForDestruction = true;
                        }
                    };
            if (this.entities_.Any(b => b is Block))
            {
                return;
            }
            
            this.entities_.Add(entity);
        }

        /// <summary>
        /// Call entities' update
        /// </summary>
        public void Update(GameTime time)
        {
            var list = entities_.ToList();
            for (var i = 0; i < entities_.Count; ++i)
            {
                list[i].Update(time);
                if (list[i] is Explosable)
                {
                    var explosable = list[i] as Explosable;
                    if (explosable.IsReadyToExplode() && Explosion != null)
                        Explosion(explosable as Bomb, base_);
                    if (explosable.MarkedForDestruction /**/&& explosable is Bomb) //TODO ==========TMP : INHIBE DESTRUCTION========================================================================
                        entities_.Remove(explosable);
                    else if (explosable.MarkedForDestruction && explosable is Block)/**/
                    {/**/
                        entities_.Remove(explosable);/**/
                        this.AddEntity(new Block(new SpriteSheet(KaboomResources.Textures["background2"], new[] {1, 2}, 2, 2), true));/**/
                    }/**/
                }
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
            var opaqueAllowed = 0;
            foreach (var entity in this.entities_.Reverse().TakeWhile(
                entity => entity.Visibility == EVisibility.Transparent ||
                    (entity.Visibility == EVisibility.Opaque && opaqueAllowed++ == 0)).Reverse())
            {
                entity.Draw(sb, t, this.base_);
            }
        }

        /// <summary>
        /// Getter for base coordinates6
        /// </summary>
        public Point Base
        {
            get { return this.base_; }
        }

        /// <summary>
        /// Set explosion marker on all bombs and blocks in the square
        /// </summary>
        public void Explode()
        {
            foreach (var entity in entities_)
            {
                if (entity as Bomb != null)
                    ((Bomb)entity).SetForExplosion(500);
                if (entity as Block != null && ((Block)entity).Destroyable)
                    ((Block)entity).SetForExplosion(50);
            }
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
