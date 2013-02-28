using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    class Bomb : Explosable
    {
        private readonly Pattern pattern_;

        /// <summary>
        /// Construct a new bomb on Z-index 10
        /// </summary>
        /// <param name="type">Pattern type</param>
        /// <param name="tile">Bomb Skin</param>
        public Bomb(Pattern.Type type, SpriteSheet tile) :
            base (3, tile, EVisibility.Transparent)
        {
            pattern_ = new Pattern(type);
        }

        public bool Merge(Bomb bomb)
        {
            this.pattern_.MergePatterns(bomb.pattern_);
            return true;
        }

        public void NextOrientation()
        {
            this.pattern_.NextOrientation();
        }

        /// <summary>
        /// Get pattern
        /// </summary>
        /// <returns>A list of points representing the explosion pattern</returns>
        public List<Pattern.PatternElement> GetPattern()
        {
            return pattern_.GetPattern();
        }

        /// <summary>
        /// Draw the bomb and its patterns on the screen
        /// </summary>
        /// <param name="sb">the spritebatch</param>
        /// <param name="t">the gametime</param>
        /// <param name="p">the position of the bomb</param>
        public override void Draw(SpriteBatch sb, GameTime t, Point p, int depth)
        {
            base.Draw(sb, t, p, 0);
            foreach (var elt in pattern_.GetPattern().Where(place => place != null).Select(place => new Point(p.X + place.Point.X, p.Y + place.Point.Y)).Where(temp => temp.X >= 0 && temp.Y >= 0))
            {
                sb.Draw(
                KaboomResources.Textures["highlight"],
                new Rectangle(
                    (elt.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                    (elt.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                    Camera.Instance.DimX,
                    Camera.Instance.DimY),
                Color.White);
            }
        }
    }
}