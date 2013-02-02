using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Kaboom.Sources
{
    class Pattern
    {
        public enum Type
        {
            Square,
            Cross
        }

        // TODO : Store time before explosion of each pos
        private static readonly Dictionary<Type, List<Point>> patterns_ = new Dictionary<Type, List<Point>>
            {
                {
                    Type.Square, new List<Point>
                        {
                            new Point(-1, -1),
                            new Point(0, -1),
                            new Point(1, -1),
                 
                            new Point(-1, 0),
                            new Point(1, 0),
                            
                            new Point(-1, 1),
                            new Point(0, 1),
                            new Point(1, 1)
                        }
                },
                {
                    Type.Cross, new List<Point>
                        {
                            new Point(0, -1),
                            new Point(-1, 0),
                            new Point(1, 0),
                            new Point(1, 0),
                        }
                }
            };

        public Type SelectedType;

        /// <summary>
        /// Initialize a class pattern with the given type, throw an exception if the type doesn't exist.
        /// </summary>
        /// <param name="type">Type of the selected pattern</param>
        public Pattern(Type type)
        {
            if (!patterns_.ContainsKey(type))
                throw new KeyNotFoundException();
                this.SelectedType = type;
        }
        
        /// <summary>
        /// GetPattern
        /// </summary>
        /// <returns>A list of points representing the explosion pattern</returns>
        public List<Point> GetPattern()
        {
            return patterns_[SelectedType];
        }
    }
}