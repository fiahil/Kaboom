using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Kaboom.Sources
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
        public bool Detroyable;
    }

    public class BombProxy : EntityProxy
    {
        public Pattern.Type Type;
    }

    public class SquareProxy
    {
        public List<EntityProxy> Entities;
    }

    public class MapElements
    {
        public readonly SquareProxy[,] Board;

        /// <summary>
        /// Initialize a new MapElement by default
        /// </summary>
        public MapElements()
        {
            this.Dimension = Point.Zero;
            this.Board = null;
        }

        /// <summary>
        /// Size of the map in square
        /// </summary>
        public Point Dimension { get; set; }
    }
}
