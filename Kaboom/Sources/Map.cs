using System;
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
                }
            }
        }

        /// <summary>
        /// Randomly fill the map with entities
        /// </summary>
        public void Randomize()
        {
            // TODO: Do a true map generator (with a random entity factory) (labyrinth generator?)
            for (var i = 0; i < this.sizeX_; i++)
            {
                for (var j = 0; j < this.sizeY_; j++)
                {
                    this.board_[i, j].AddEntity(new UnitestEntity(0,
                                                                  new SpriteSheet(
                                                                      KaboomResources.Textures["background1"],
                                                                      new[] {1})));
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
        public void AddNewEntity(IEntity entity, Point coordinates)
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
            foreach (var square in this.board_)
            {
                var r = new Rectangle(
                    (square.Base.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                    (square.Base.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                    Camera.Instance.DimX,
                    Camera.Instance.DimY);

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
    }
}