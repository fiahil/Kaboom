using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Kaboom.Sources
{
    class Bomb : Explosable
    {
        private readonly Pattern pattern_;
        private readonly string highlight_;

        private readonly Dictionary<Pattern.Type, string> spriteCorrespondance_ = new Dictionary<Pattern.Type, string>
            {
                {Pattern.Type.BigSquare, "BombBigSquare"},
                {Pattern.Type.H, "BombH"},
                {Pattern.Type.X, "BombX"},
                {Pattern.Type.Ultimate, "BombUltimate"},
            };

        /// <summary>
        /// Construct a new bomb on Z-index 10
        /// </summary>
        /// <param name="type">Pattern type</param>
        /// <param name="tile">Bomb Skin</param>
        /// <param name="highlight"> </param>
        /// <param name="orientation"> orientation of the current pattern </param>
        public Bomb(Pattern.Type type, SpriteSheet tile, string highlight = "highlight", int orientation = 0)
            : base(3, tile, EVisibility.Transparent)
        {
            highlight_ = highlight;
            pattern_ = new Pattern(type, orientation);
        }

        /// <summary>
        /// Try to merge its pattern with the pattern of the given bomb
        /// </summary>
        /// <param name="bomb">Bomb to merge with the object</param>
        /// <returns>Merge succeded or not</returns>
        public bool Merge(Bomb bomb)
        {
            if (pattern_.MergePatterns(bomb.pattern_))
            {
                if (spriteCorrespondance_.ContainsKey(pattern_.SelectedType))
                    Tile = KaboomResources.Sprites[spriteCorrespondance_[pattern_.SelectedType]].Clone() as SpriteSheet;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Rotate bomb
        /// </summary>
        public void NextOrientation()
        {
            pattern_.NextOrientation();
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
       /// Return the orientation of the current pattern
       /// </summary>
       /// <returns></returns>
        public int GetPatternOrientation()
        {
            return pattern_.GetOrientation();
        }

        /// <summary>
        /// Draw the bomb and its patterns on the screen
        /// </summary>
        /// <param name="sb">the spritebatch</param>
        /// <param name="t">the gametime</param>
        /// <param name="p">the position of the bomb</param>
        public override void Draw(SpriteBatch sb, GameTime t, Point p)
        {
            base.Draw(sb, t, p);
            var mapDimension = Viewport.Instance.MapDimensions();
            if (highlight_ != "")
                foreach (
                    var elt in
                        pattern_.GetPattern().Where(place => place != null).Select(
                            place => new Point(p.X + place.Point.X, p.Y + place.Point.Y)).Where(
                                temp => temp.X >= 0 && temp.Y >= 0 && temp.X < mapDimension.X && temp.Y < mapDimension.Y)
                    )
                {
                    sb.Draw(
                        KaboomResources.Textures[highlight_],
                        new Rectangle(
                            (elt.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                            (elt.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                            Camera.Instance.DimX,
                            Camera.Instance.DimY),
                        KaboomResources.Textures[highlight_].Bounds,
                        Color.White,
                        0.0f,
                        Vector2.Zero,
                        0,
                        0.01f);
                }
        }
    }
}