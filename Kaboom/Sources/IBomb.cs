using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Kaboom.Sources
{
    interface IBomb
    {
        /// <summary>
        /// Specifies whether the bomb must explode or not
        /// </summary>
        /// <returns></returns>
        bool IsReadyToExplode();

        /// <summary>
        /// Get pattern
        /// </summary>
        /// <returns>A list of points representing the explosion pattern</returns>
        List<Point> GetPattern();

        /// <summary>
        /// Start the timer before the bomb explodes
        /// </summary>
        /// <param name="time">Time to wait before explosion (in milliseconds)</param>
        void SetForExplosion(double time);
    }
}