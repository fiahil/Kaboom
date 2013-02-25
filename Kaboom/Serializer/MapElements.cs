using System.Collections.Generic;
using System.Xml.Serialization;

namespace Kaboom.Serializer
{
    public class EntityProxy
    {
        public int ZIndex;
        public string TileIdentifier;
        public int[] TileFramePerAnim;
        public int TileFrameSpeed;
        public int TileTotalAnim;
    }

    public class BlockProxy : EntityProxy
    {
        public bool Destroyable;
    }

    public class BombProxy : EntityProxy
    {
        public int Type;
    }

    public class SquareProxy
    {
        public List<EntityProxy> Entities;

        public SquareProxy()
        {
            this.Entities = new List<EntityProxy>();
        }
    }

    [XmlInclude(typeof(BlockProxy)), XmlInclude(typeof(BombProxy))]
    public class MapElements
    {
        public SquareProxy[][] Board;
        public int SizeX;
        public int SizeY;

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
