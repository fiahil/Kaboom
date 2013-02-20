using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    internal class Map : DrawableGameComponent
    {

        private readonly Square[,] board_;
        private readonly SpriteBatch sb_;
        private readonly int sizeX_;
        private readonly int sizeY_;

        /// <summary>
        /// Initialize a new map
        /// </summary>
        /// <param name="g">Game instance</param>
        /// <param name="sb">Spritebatch used to draw textures</param>
        /// <param name="sizex">X map number of entities</param>
        /// <param name="sizey">Y map number of entities</param>
        public Map(Game g, SpriteBatch sb, int sizex, int sizey)
            : base(g)
        {
            this.sb_ = sb;
            this.board_ = new Square[sizex,sizey];
            this.sizeX_ = sizex;
            this.sizeY_ = sizey;

            for (var i = 0; i < this.sizeX_; i++)
            {
                for (var j = 0; j < this.sizeY_; j++)
                {
                    this.board_[i, j] = new Square(new Point(i, j));
                    this.board_[i, j].Explosion += ExplosionRuler;
                }
            }
        }

        /// <summary>
        /// Randomly fill the map with entities
        /// </summary>
        public void Randomize()
        {
            // TODO: Do a true map generator (with a random entity factory) (labyrinth generator?)
            var r = new Random();

            for (var i = 0; i < this.sizeX_; i++)
            {
                for (var j = 0; j < this.sizeY_; j++)
                {
                    this.board_[i, j].AddEntity(new Entity(0, new SpriteSheet(KaboomResources.Textures["background1"], new[] { 1 }, 1)));

                    if (i != 0 || j != 0)
                    {
                        this.board_[i, j].AddEntity(r.Next(2) == 0
                                                        ? new Block(
                                                              new SpriteSheet(KaboomResources.Textures["background2"],
                                                                              new[] { 4, 4 }, 2, 8), true)
                                                        : new Block(
                                                              new SpriteSheet(KaboomResources.Textures["background3"],
                                                                              new[] { 1 }, 1, 1), false));
                    }
                }
            }
        }

        /// <summary>
        /// Initialize a new map by unserialization or randomization
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            Randomize();
        }

        /// <summary>
        /// Place an entity on the map
        /// </summary>
        /// <param name="entity">the entity to place on the map</param>
        /// <param name="coordinates">coordinates of the entity</param>
        public void AddNewEntity(Entity entity, Point coordinates)
        {
            this.board_[coordinates.X, coordinates.Y].AddEntity(entity);
        }

        /// <summary>
        /// Return the coordinates of the square matching the given position
        /// Throw IndexOutOfRangeException when position outside the board
        /// </summary>
        /// <param name="position">Position to poll</param>
        /// <returns>Matching coordinates</returns>
        public Point GetCoordByPos(Vector2 position)
        {
            var r = new Rectangle(0, 0, Camera.Instance.DimX, Camera.Instance.DimY);
            foreach (var square in this.board_)
            {
                r.X = (square.Base.X * Camera.Instance.DimX) + Camera.Instance.OffX;
                r.Y = (square.Base.Y * Camera.Instance.DimY) + Camera.Instance.OffY;

                if (r.Contains(new Point((int) position.X, (int) position.Y)))
                    return square.Base;
            }
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Return the highest Z-Index for the square matching the given coordinates
        /// </summary>
        /// <param name="point">Coordinates</param>
        /// <returns>Highest Z-Index found at this position. -1 if no entities on the square</returns>
        public int GetMaxZIndexOnCoord(Point point)
        {
            return this.board_[point.X, point.Y].GetMaxZIndex();
        }

        /// <summary>
        /// Update all map's entities
        /// </summary>
        /// <param name="gameTime">GameClock</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var item in this.board_)
            {
                item.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw all map's entities. Call SpriteBatch's begin and end
        /// </summary>
        /// <param name="gameTime">GameClock</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.sb_.Begin();
            foreach (var item in this.board_)
            {
                item.Draw(this.sb_, gameTime);
            }
            this.sb_.End();
        }

        /// <summary>
        /// Deleguate for explosion
        /// </summary>
        /// <param name="bomb">Bomb ready to boom</param>
        /// <param name="pos">Position of the bomb</param>
        public void ExplosionRuler(Bomb bomb, Point pos)
        {
            if (bomb == null)
                return;

            var t = new Rectangle(0, 0, sizeX_, sizeY_);
            foreach (var touchedPos in
                from position in bomb.GetPattern()
                select new Point(pos.X + position.X, pos.Y + position.Y)
                into touchedPos
                where t.Contains(touchedPos)
                select touchedPos)
            {
                board_[touchedPos.X, touchedPos.Y].Explode();
            }
        }
        
        /// <summary>
        /// Launch an explosion on given position
        /// </summary>
        /// <param name="pos">Position to start explosion</param>
        public void SetExplosion(Point pos)
        {
            board_[pos.X, pos.Y].Explode();
        }
    }
}