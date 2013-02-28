using System.Collections.Generic;

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

        /// <summary>
        /// Try to merge its pattern with the pattern of the given bomb
        /// </summary>
        /// <param name="bomb">Bomb to merge with the object</param>
        /// <returns>Merge succeded or not</returns>
        public bool Merge(Bomb bomb)
        {
            return this.pattern_.MergePatterns(bomb.pattern_);
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
    }
}