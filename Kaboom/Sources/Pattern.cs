using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Kaboom.Sources
{
    public class Pattern
    {
        public enum Type
        {
            Square,
            Line,
            Angle,
            BigSquare,
            H,
            X,
            Ultimate,
            NoPattern
        }

        public static Type[] All = new[]
            {
                Type.Square, Type.Line, Type.Angle, Type.BigSquare, Type.H, Type.X, Type.Ultimate
            };

        /// <summary>
        /// Describe a point in a pattern
        /// </summary>
        public class PatternElement
        {
            public Point Point;
            public double Time;

            /// <summary>
            /// Initilize a new PatternElement
            /// </summary>
            /// <param name="x">X coordinate</param>
            /// <param name="y">Y coordinate</param>
            /// <param name="time">trigger in milliseconds</param>
            public PatternElement(int x, int y, double time = 500)
            {
                this.Point = new Point(x, y);
                this.Time = time;
            }
        }

        /// <summary>
        /// Pattern List
        /// </summary>
        private static readonly Dictionary<Type, List<List<PatternElement>>> patterns_ = new Dictionary
            <Type, List<List<PatternElement>>>
            {
                #region Square
                {
                    Type.Square, new List<List<PatternElement>>
                        {
                            new List<PatternElement>
                                {
                                    new PatternElement(-1, 0),
                                    new PatternElement(0, 0),
                                    new PatternElement(1, 0),
                                    new PatternElement(0, -1),
                                    new PatternElement(0, 1)
                                }
                        }
                },

                #endregion

                #region Line
                {
                    Type.Line, new List<List<PatternElement>>
                        {
                            #region Vertical
                            new List<PatternElement>
                                {
                                    new PatternElement(-2, 0),
                                    new PatternElement(-1, 0),
                                    new PatternElement(0, 0),
                                    new PatternElement(1, 0),
                                    new PatternElement(2, 0)
                                },
                            #endregion

                            #region Horizontal
                            new List<PatternElement>
                                {
                                    new PatternElement(0, -2),
                                    new PatternElement(0, -1),
                                    new PatternElement(0, 0),
                                    new PatternElement(0, 1),
                                    new PatternElement(0, 2)
                                }
                            #endregion

                        }
                },

                #endregion

                #region Angle
                {
                    Type.Angle, new List<List<PatternElement>>
                        {
                            #region BottomLeft
                            new List<PatternElement>
                                {
                                    new PatternElement(0, 0, 500),
                                    new PatternElement(-1, 0, 750),
                                    new PatternElement(-2, 0, 1000),
                                    new PatternElement(0, 1, 750),
                                    new PatternElement(0, 2, 1000),
                                    new PatternElement(-1, 1, 1000)
                                },

                            #endregion

                            #region TopLeft
                            new List<PatternElement>
                                {
                                    new PatternElement(-2, 0, 1000),
                                    new PatternElement(-1, 0, 750),
                                    new PatternElement(0, 0, 500),
                                    new PatternElement(0, -1, 750),
                                    new PatternElement(0, -2, 1000),
                                    new PatternElement(-1, -1, 1000)
                                },

                            #endregion

                            #region TopRight
                            new List<PatternElement>
                                {
                                    new PatternElement(2, 0, 1000),
                                    new PatternElement(1, 0, 750),
                                    new PatternElement(0, 0, 500),
                                    new PatternElement(0, -1, 750),
                                    new PatternElement(0, -2, 1000),
                                    new PatternElement(1, -1, 1000)
                                },

                            #endregion
                                
                            #region BottomRight
                            new List<PatternElement>
                                {
                                    new PatternElement(2, 0, 1000),
                                    new PatternElement(1, 0, 750),
                                    new PatternElement(0, 0, 500),
                                    new PatternElement(0, 1, 750),
                                    new PatternElement(0, 2, 1000),
                                    new PatternElement(1, 1, 1000)
                                }

                            #endregion

                        }
                },
                #endregion

                #region H
                {
                    Type.H, new List<List<PatternElement>>
                        {
                            #region Horizontal
                            new List<PatternElement>
                                {
                                    new PatternElement(0, 0),

                                    new PatternElement(0, 1),
                                    new PatternElement(0, 2),
                                    new PatternElement(-1, 2),
                                    new PatternElement(-2, 2),
                                    new PatternElement(1, 2),
                                    new PatternElement(2, 2),

                                    new PatternElement(0, -1),
                                    new PatternElement(0, -2),
                                    new PatternElement(-1, -2),
                                    new PatternElement(-2, -2),
                                    new PatternElement(1, -2),
                                    new PatternElement(2, -2)
                                },
                            #endregion

                            #region Vertical
                            new List<PatternElement>
                                {
                                    new PatternElement(0, 0),

                                    new PatternElement(1, 0),
                                    new PatternElement(2, 0),
                                    new PatternElement(2, 1),
                                    new PatternElement(2, 2),
                                    new PatternElement(2, -1),
                                    new PatternElement(2, -2),

                                    new PatternElement(-1, 0),
                                    new PatternElement(-2, 0),
                                    new PatternElement(-2, 1),
                                    new PatternElement(-2, 2),
                                    new PatternElement(-2, -1),
                                    new PatternElement(-2, -2)
                                }

                            #endregion
                        }
                },

                #endregion

                #region BigSquare
                {
                    Type.BigSquare, new List<List<PatternElement>>
                        {
                            new List<PatternElement>
                                {
                                    new PatternElement(0, 2),
                                    new PatternElement(-1, 1),
                                    new PatternElement(0, 1),
                                    new PatternElement(1, 1),

                                    new PatternElement(0, -2),
                                    new PatternElement(-1, -1),
                                    new PatternElement(0, -1),
                                    new PatternElement(1, -1),

                                    new PatternElement(-2, 0),
                                    new PatternElement(-1, 0),
                                    new PatternElement(0, 0),
                                    new PatternElement(1, 0),
                                    new PatternElement(2, 0)
                                }
                        }
                },

                #endregion

                #region X
                {
                    Type.X, new List<List<PatternElement>>
                        {
                            new List<PatternElement>
                                {
                                    new PatternElement(0, 0),

                                    new PatternElement(-1, -1),
                                    new PatternElement(-2, -2),
                                    new PatternElement(-3, -3),

                                    new PatternElement(1, 1),
                                    new PatternElement(2, 2),
                                    new PatternElement(3, 3),

                                    new PatternElement(1, -1),
                                    new PatternElement(2, -2),
                                    new PatternElement(3, -3),

                                    new PatternElement(-1, 1),
                                    new PatternElement(-2, 2),
                                    new PatternElement(-3, 3)
                                }
                        }
                },
                #endregion

                #region Ultimate
                {
                    Type.Ultimate, new List<List<PatternElement>>
                        {
                            new List<PatternElement>
                                {
                                    new PatternElement(0, 0),

                                    new PatternElement(-6, 0),
                                    new PatternElement(-5, 0),
                                    new PatternElement(-4, 0),

                                    new PatternElement(6, 0),
                                    new PatternElement(5, 0),
                                    new PatternElement(4, 0),

                                    new PatternElement(0, -6),
                                    new PatternElement(0, -5),
                                    new PatternElement(0, -4),

                                    new PatternElement(0, 6),
                                    new PatternElement(0, 5),
                                    new PatternElement(0, 4),

                                    new PatternElement(-3, 3),
                                    new PatternElement(-4, 3),
                                    new PatternElement(-3, 4),

                                    new PatternElement(3, 3),
                                    new PatternElement(4, 3),
                                    new PatternElement(3, 4),

                                    new PatternElement(3, -3),
                                    new PatternElement(4, -3),
                                    new PatternElement(3, -4),

                                    new PatternElement(-3, -3),
                                    new PatternElement(-4, -3),
                                    new PatternElement(-3, -4)
                                }
                        }
                }

                #endregion
            };

        private int orientation_;

        /// <summary>
        /// Initialize a class pattern with the given type, throw an exception if the type doesn't exist.
        /// </summary>
        /// <param name="type">Type of the selected pattern</param>
        public Pattern(Type type)
        {
            if (!patterns_.ContainsKey(type))
                throw new KeyNotFoundException();
            this.SelectedType = type;
            this.orientation_ = 0;
        }

        public Type SelectedType
        {
            get;
            private set;
        }

        public void NextOrientation()
        {
            this.orientation_ = (this.orientation_ + 1) % patterns_[this.SelectedType].Count;
        }
        
        /// <summary>
        /// GetPattern
        /// </summary>
        /// <returns>A list of points representing the explosion pattern</returns>
        public List<PatternElement> GetPattern()
        {
            return patterns_[this.SelectedType][this.orientation_];
        }
    }
}