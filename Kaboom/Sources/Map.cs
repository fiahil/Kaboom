using System;
using System.Linq;
using Kaboom.Serializer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    class Map : DrawableGameComponent
    {
        private readonly Square[,] board_;
        private readonly SpriteBatch sb_;
        private readonly int turnsRemeaning_;
        public event EventHandler EndGame;

        /// <summary>
        /// Initialize a new map from a MapElements
        /// </summary>
        /// <param name="g"></param>
        /// <param name="sb"></param>
        /// <param name="me"></param>
        public Map(Game g, SpriteBatch sb, MapElements me)
            : base(g)
        {
            this.sb_ = sb;
            this.SizeX = me.SizeX;
            this.SizeY = me.SizeY;
            this.turnsRemeaning_ = 5;

            this.board_ = new Square[this.SizeX,this.SizeY];
            for (var i = 0; i < this.SizeX; i++)
            {
                for (var j = 0; j < this.SizeY; j++)
                {
                    this.board_[i, j] = new Square(new Point(i, j));
                    this.board_[i, j].Explosion += ExplosionRuler;
                    this.board_[i, j].EndGame +=
                    (sender, ea) =>
                        {
                            if (EndGame != null) 
                                EndGame(sender, ea);
                        };

                    var typeArray = Pattern.All;

                    foreach (var entity in me.Board[i][j].Entities)
                    {
                        var bombProxy = entity as BombProxy;
                        var blockProxy = entity as BlockProxy;
                        var checkPointProxy = entity as CheckPointProxy;

                        if (bombProxy != null)
                        {
                            this.board_[i, j].AddEntity(
                                new Bomb(typeArray[bombProxy.Type],
                                         new SpriteSheet(
                                             KaboomResources.Textures[bombProxy.TileIdentifier],
                                             bombProxy.TileFramePerAnim,
                                             bombProxy.TileTotalAnim,
                                             bombProxy.TileFrameSpeed), "" ));
                        }

                        if (checkPointProxy != null)
                        {
                            this.board_[i, j].AddEntity(
                                new CheckPoint(new SpriteSheet(
                                    KaboomResources.Textures[checkPointProxy.TileIdentifier], 
                                    checkPointProxy.TileFramePerAnim,
                                    checkPointProxy.TileTotalAnim,
                                    checkPointProxy.TileFrameSpeed), 500));
                        }

                        if (blockProxy != null)
                        {
                            this.board_[i, j].AddEntity(
                                new Block(new SpriteSheet(
                                              KaboomResources.Textures[blockProxy.TileIdentifier],
                                              blockProxy.TileFramePerAnim,
                                              blockProxy.TileTotalAnim,
                                              blockProxy.TileFrameSpeed), blockProxy.Destroyable, blockProxy.GameEnd));
                        }
                        else
                        {
                            this.board_[i, j].AddEntity(
                                new Entity(entity.ZIndex,
                                           new SpriteSheet(
                                               KaboomResources.Textures[entity.TileIdentifier],
                                               entity.TileFramePerAnim,
                                               entity.TileTotalAnim,
                                               entity.TileFrameSpeed)));
                        }
                    }
                }
            }
        }

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
            this.SizeX = sizex;
            this.SizeY = sizey;

            for (var i = 0; i < this.SizeX; i++)
            {
                for (var j = 0; j < this.SizeY; j++)
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

            for (var i = 0; i < this.SizeX; i++)
            {
                for (var j = 0; j < this.SizeY; j++)
                {
                    this.board_[i, j].AddEntity(new Entity(1, KaboomResources.Sprites["Ground"].Clone() as SpriteSheet));

                    if (i != 7 || j != 7)
                    {
                        this.board_[i, j].AddEntity(r.Next(2) != 0
                                                        ? new Block(
                                                              KaboomResources.Sprites["DestructibleBlock"].Clone() as SpriteSheet, true)
                                                        : new Block(
                                                              KaboomResources.Sprites["UndestructibleBlock"].Clone() as SpriteSheet, false));
                    }
                }
            }
        }

        /// <summary>
        /// Test if the line in posY is contain in the rectangle
        /// </summary>
        /// <param name="vision">Testing rectangle</param>
        /// <param name="posY">Testing line</param>
        /// <returns></returns>
        public Boolean LineIsContainHorizontaly(Rectangle vision, int posY)
        {
            for (var i = 0; i < this.SizeX; i++)
            {
                if (
                    vision.Contains(
                        new Rectangle((board_[i, posY].Base.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                                      (board_[i, posY].Base.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                                      Camera.Instance.DimX, Camera.Instance.DimY)))
                {
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Test if the column in posX is contain in the rectangle
        /// </summary>
        /// <param name="vision">Testing rectangle</param>
        /// <param name="posX">Testing column</param>
        /// <returns></returns>
        public Boolean LineIsContainVerticaly(Rectangle vision, int posX)
        {
            for (var i = 0; i < this.SizeX; i++)
            {
                if (
                    vision.Contains(
                        new Rectangle((board_[posX, i].Base.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                                      (board_[posX, i].Base.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                                      Camera.Instance.DimX, Camera.Instance.DimY)))
                {
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Place an entity on the map
        /// </summary>
        /// <param name="entity">the entity to place on the map</param>
        /// <param name="coordinates">coordinates of the entity</param>
        public bool AddNewEntity(Entity entity, Point coordinates)
        {
            return this.board_[coordinates.X, coordinates.Y].AddEntity(entity);
        }

        public void RemoveEntity(Point coordinates, int offset = 5)
        {
            this.board_[coordinates.X, coordinates.Y].RemoveEntity(offset);
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

            this.sb_.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
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

            var t = new Rectangle(0, 0, this.SizeX, this.SizeY);

            foreach (var elt in bomb.GetPattern().Where(elt => t.Contains(new Point(pos.X + elt.Point.X, pos.Y + elt.Point.Y))))
            {
                this.board_[pos.X + elt.Point.X, pos.Y + elt.Point.Y].Explode(elt.Time);
            }
        }
        
        public void ActivateDetonators()
        {
            foreach (var square in board_)
            {
                square.ActiveDetonator();
            }    
        }

        /// <summary>
        /// Launch an explosion on given position
        /// </summary>
        /// <param name="pos">Position to start explosion</param>
        public void SetExplosion(Point pos)
        {
            board_[pos.X, pos.Y].Explode(500);
        }

        /// <summary>
        /// Dimension of the map (in square)
        /// </summary>
        public int SizeX { get; private set; }

        /// <summary>
        /// Dimension of the map (in square)
        /// </summary>
        public int SizeY { get; private set; }
    }
}