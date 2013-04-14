using System.Collections.Generic;
using System.Xml.Serialization;

namespace Kaboom.Serializer
{
    /// <summary>
    /// Entity Proxy Class
    /// Used to store parameters for instanciation
    /// </summary>
    public class EntityProxy
    {
        public int ZIndex;
        public string TileIdentifier;
        public int[] TileFramePerAnim;
        public int TileFrameSpeed;
        public int TileTotalAnim;

        public virtual EntityProxy Clone()
        {
            return new EntityProxy
                {
                    ZIndex = this.ZIndex,
                    TileIdentifier = this.TileIdentifier,
                    TileFramePerAnim = this.TileFramePerAnim,
                    TileFrameSpeed = this.TileFrameSpeed,
                    TileTotalAnim = this.TileTotalAnim
                };
        }
    }

    /// <summary>
    /// Block Proxy Class
    /// Used to store parameters for instanciation
    /// </summary>
    public class BlockProxy : EntityProxy
    {
        public bool Destroyable;
        public bool GameEnd = false;

        public override EntityProxy Clone()
        {
            return new BlockProxy
                {
                    GameEnd = this.GameEnd,
                    Destroyable = this.Destroyable,
                    ZIndex = this.ZIndex,
                    TileIdentifier = this.TileIdentifier,
                    TileFramePerAnim = this.TileFramePerAnim,
                    TileFrameSpeed = this.TileFrameSpeed,
                    TileTotalAnim = this.TileTotalAnim
                };
        }
    }

    /// <summary>
    /// Bomb Proxy Class
    /// Used to store parameters for instanciation
    /// </summary>
    public class BombProxy : EntityProxy
    {
        public int Type;

        public override EntityProxy Clone()
        {
            return new BombProxy
                {
                    Type = this.Type,
                    ZIndex = this.ZIndex,
                    TileIdentifier = this.TileIdentifier,
                    TileFramePerAnim = this.TileFramePerAnim,
                    TileFrameSpeed = this.TileFrameSpeed,
                    TileTotalAnim = this.TileTotalAnim
                };
        }
    }

    /// <summary>
    /// Checkpoint Proxy Class
    /// Used to store parameters for instanciation
    /// </summary>
    public class CheckPointProxy : EntityProxy
    {
        public int Bombsetidx;
        public bool Activated;

        public override EntityProxy Clone()
        {
            return new CheckPointProxy
                {
                    Activated = this.Activated,
                    Bombsetidx = this.Bombsetidx,
                    ZIndex = this.ZIndex,
                    TileIdentifier = this.TileIdentifier,
                    TileFramePerAnim = this.TileFramePerAnim,
                    TileTotalAnim = this.TileTotalAnim,
                    TileFrameSpeed = this.TileFrameSpeed
                };
        }
    }

    /// <summary>
    /// Square Proxy Class
    /// Store proxy classes
    /// </summary>
    public class SquareProxy
    {
        public List<EntityProxy> Entities;

        public SquareProxy()
        {
            this.Entities = new List<EntityProxy>();
        }
    }

    /// <summary>
    /// Bombset Proxy Class
    /// Store bombset
    /// </summary>
    public class BombInfoProxy
    {
        private int quantity_;
        private int type_;
        private string name_;

        public int Quantity
        {
            get { return quantity_; }
            set { quantity_ = value; }
        }

        public int Type
        {
            get { return type_; }
            set { type_ = value; }
        }

        public string Name
        {
            get { return name_; }
            set { name_ = value; }
        }
    }

    public class ScoreInfoProxy
    {
        private int turn_ = 1;
        private int score1_;
        private int score2_;
        private int score3_;

        public int Turn
        {
            get { return turn_; }
            set { turn_ = value; }
        }

        public int Score1
        {
            get { return score1_; }
            set { score1_ = value; }
        }

        public int Score2
        {
            get { return score2_; }
            set { score2_ = value; }
        }

        public int Score3
        {
            get { return score3_; }
            set { score3_ = value; }
        }
    }

    /// <summary>
    /// Used to convay a map over xml serialization
    /// </summary>
    [XmlInclude(typeof(BlockProxy)), XmlInclude(typeof(BombProxy)), XmlInclude(typeof(CheckPointProxy))]
    public class MapElements
    {
        public SquareProxy[][] Board;
        public List<List<BombInfoProxy>> Bombset; 
        public int SizeX;
        public int SizeY;
        public ScoreInfoProxy Score;


        /// <summary>
        /// Initialize a new MapElement by default
        /// </summary>
        public MapElements()
        {
            if (Score == null)
                Score = new ScoreInfoProxy();
        }

        /// <summary>
        /// Initialize a new MapElement
        /// </summary>
        /// <param name="x">X size of the map (in square)</param>
        /// <param name="y">Y size of the map (in square)</param>
        public MapElements(int x, int y)
        {
            this.SizeX = x;
            this.SizeY = y;
            this.Board = new SquareProxy[x][];
            this.Score = new ScoreInfoProxy();

            for (var i = 0; i < x; i++)
            {
                this.Board[i] = new SquareProxy[y];
                for (var j = 0; j < y; j++)
                {
                    this.Board[i][j] = new SquareProxy();
                }
            }
            this.Bombset = new List<List<BombInfoProxy>> {new List<BombInfoProxy>()};
        }
    }
}
