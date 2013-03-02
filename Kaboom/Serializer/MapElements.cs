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
    /// Checkpoint Proxy Class
    /// Used to store parameters for instanciation
    /// </summary>
    public class CheckPointProxy : EntityProxy
    {
        // TODO : Ajouter le pool de bombe
    }

    /// <summary>
    /// Used to convay a map over xml serialization
    /// </summary>
    [XmlInclude(typeof(BlockProxy)), XmlInclude(typeof(BombProxy)), XmlInclude(typeof(CheckPointProxy))]
    public class MapElements
    {
        public SquareProxy[][] Board;
        public int SizeX;
        public int SizeY;
        public int EndPosX;
        public int EndPosY;

        /// <summary>
        /// Initialize a new MapElement by default
        /// </summary>
        public MapElements()
        {
            this.SizeX = 0;
            this.SizeY = 0;
            this.Board = null;
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

            for (var i = 0; i < x; i++)
            {
                this.Board[i] = new SquareProxy[y];
                for (var j = 0; j < y; j++)
                {
                    this.Board[i][j] = new SquareProxy();
                }
            }
        }
    }
}
