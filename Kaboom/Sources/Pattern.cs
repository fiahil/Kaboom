using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Kaboom.Sources
{
    class Pattern
    {
        public enum Type
        {
            Square,
            LineH,
            LineV,
            AngleT,
            AngleB,
            AngleL,
            AngleR,
            BigSquare,
            HfV,
            HfH,
            X,
            Ultimate
        }

        // TODO : Store time before explosion of each pos
        private static readonly Dictionary<Type, List<Point>> patterns_ = new Dictionary<Type, List<Point>>
            {
                {
                    Type.Square, new List<Point>
                        {
                            new Point(-1, 0),
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(0, -1),
                            new Point(0, 1)
                        }
                },
                {
                    Type.LineH, new List<Point>
                        {
                            new Point(-2, 0),
                            new Point(-1, 0),
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(2, 0)
                        }
                },
                {
                    Type.LineV, new List<Point>
                        {
                            new Point(0, -2),
                            new Point(0, -1),
                            new Point(0, 0),
                            new Point(0, 1),
                            new Point(0, 2)
                        }
                },
                {
                    Type.AngleL, new List<Point>
                        {
                            new Point(0, 0),
                            new Point(-1, 0),
                            new Point(-2, 0),
                            new Point(0, 1),
                            new Point(0, 2),
                            new Point(-1, 1)
                        }
                },
                {
                    Type.AngleT, new List<Point>
                        {
                            new Point(-2, 0),
                            new Point(-1, 0),
                            new Point(0, 0),
                            new Point(0, -1),
                            new Point(0, -2),
                            new Point(-1, -1)
                        }
                },
                {
                    Type.AngleB, new List<Point>
                        {
                            new Point(0, 2),
                            new Point(0, 1),
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(2, 0),
                            new Point(1, 1)
                        }
                },
                {
                    Type.AngleR, new List<Point>
                        {
                            new Point(2, 0),
                            new Point(1, 0),
                            new Point(0, 0),
                            new Point(0, -1),
                            new Point(0, -2),
                            new Point(1, -1)
                        }
                },
                {
                    Type.BigSquare, new List<Point>
                        {
                            new Point(0, 2),
                            new Point(-1, 1),
                            new Point(0, 1),
                            new Point(1, 1),

                            new Point(0, -2),
                            new Point(-1, -1),
                            new Point(0, -1),
                            new Point(1, -1),

                            new Point(-2, 0),
                            new Point(-1, 0),
                            new Point(0, 0),
                            new Point(1, 0),
                            new Point(2, 0)
                        }
                },
                {
                    Type.HfH, new List<Point>
                        {
                            new Point(0, 0),

                            new Point(0, 1),
                            new Point(0, 2),
                            new Point(-1, 2),
                            new Point(-2, 2),
                            new Point(1, 2),
                            new Point(2, 2),

                            new Point(0, -1),
                            new Point(0, -2),
                            new Point(-1, -2),
                            new Point(-2, -2),
                            new Point(1, -2),
                            new Point(2, -2)
                        }
                },
                {
                    Type.HfV, new List<Point>
                        {
                            new Point(0, 0),

                            new Point(1, 0),
                            new Point(2, 0),
                            new Point(2, 1),
                            new Point(2, 2),
                            new Point(2, -1),
                            new Point(2, -2),

                            new Point(-1, 0),
                            new Point(-2, 0),
                            new Point(-2, 1),
                            new Point(-2, 2),
                            new Point(-2, -1),
                            new Point(-2, -2)
                        }
                },
                {
                    Type.X, new List<Point>
                        {
                            new Point(0, 0),

                            new Point(-1, -1),
                            new Point(-2, -2),
                            new Point(-3, -3),
                            
                            new Point(1, 1),
                            new Point(2, 2),
                            new Point(3, 3),

                            new Point(1, -1),
                            new Point(2, -2),
                            new Point(3, -3),

                            new Point(-1, 1),
                            new Point(-2, 2),
                            new Point(-3, 3)
                        }
                },
                {
                    Type.Ultimate, new List<Point>
                        {
                            new Point(0, 0),

                            new Point(-6, 0),
                            new Point(-5, 0),
                            new Point(-4, 0),

                            new Point(6, 0),
                            new Point(5, 0),
                            new Point(4, 0),

                            new Point(0, -6),
                            new Point(0, -5),
                            new Point(0, -4),

                            new Point(0, 6),
                            new Point(0, 5),
                            new Point(0, 4),

                            new Point(-3, 3),
                            new Point(-4, 3),
                            new Point(-3, 4),

                            new Point(3, 3),
                            new Point(4, 3),
                            new Point(3, 4),

                            new Point(3, -3),
                            new Point(4, -3),
                            new Point(3, -4),

                            new Point(-3, -3),
                            new Point(-4, -3),
                            new Point(-3, -4)
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