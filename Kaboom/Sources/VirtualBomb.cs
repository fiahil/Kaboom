using System;
using System.Collections.Generic;
using System.Text;

namespace Kaboom.Sources
{
    class VirtualBomb : Bomb
    {
        private readonly Pattern.Type type_;
        private readonly SpriteSheet tile_;

        public VirtualBomb(Pattern.Type type, SpriteSheet tile)
            : base(type, tile, "highlight2")
        {
            type_ = type;
            tile_ = tile;
        }

        public Bomb ToBomb()
        {
            return new Bomb(type_, tile_, "highlight", this.GetPatternOrientation());
        }
    }
}
