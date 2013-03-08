using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Kaboom.Sources
{
    public class Pattern
    {
        public enum Type
        {
            Square,
            Tnt,
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
                Type.Square, Type.Tnt, Type.Line, Type.Angle, Type.BigSquare, Type.H, Type.X, Type.Ultimate
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

                #region TNT
                {
                    Type.Tnt, new List<List<PatternElement>>
                        {
                            new List<PatternElement>
                                {
                                    new PatternElement(-1, 0),
                                    new PatternElement(-1, -1),
                                    new PatternElement(0, 0),
                                    new PatternElement(1, 0),
                                    new PatternElement(1, 1),
                                    new PatternElement(0, -1),
                                    new PatternElement(1, -1),
                                    new PatternElement(0, 1),
                                    new PatternElement(-1, 1)
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
                                    new PatternElement(0, 0),
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
                                    new PatternElement(0, 0),
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
                                    new PatternElement(0, 0),
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
                                    new PatternElement(0, 0),
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

        /// <summary>
        /// Pattern merging table
        /// </summary>
        private static readonly Dictionary<KeyValuePair<Type, Type>, Type> mergings_ = new Dictionary
            <KeyValuePair<Type, Type>, Type>
            {
                #region Square Merging
                { new KeyValuePair<Type, Type>(Type.Square, Type.Square), Type.BigSquare },
                { new KeyValuePair<Type, Type>(Type.Square, Type.Angle), Type.X },
                { new KeyValuePair<Type, Type>(Type.Square, Type.Line), Type.H },
                #endregion
            
                #region Line Merging
                { new KeyValuePair<Type, Type>(Type.Line, Type.Angle), Type.X },
                { new KeyValuePair<Type, Type>(Type.Line, Type.Line), Type.H },
                #endregion

                { new KeyValuePair<Type, Type>(Type.BigSquare, Type.Angle), Type.Ultimate },
                { new KeyValuePair<Type, Type>(Type.BigSquare, Type.Line), Type.Ultimate },
                { new KeyValuePair<Type, Type>(Type.X, Type.Square), Type.Ultimate },
                { new KeyValuePair<Type, Type>(Type.H, Type.Square), Type.Ultimate },

            };

        private int orientation_;

        /// <summary>
        /// Initialize a class pattern with the given type, throw an exception if the type doesn't exist.
        /// </summary>
        /// <param name="type">Type of the selected pattern</param>
        /// <param name="orientation"> </param>
        public Pattern(Type type, int orientation = 0)
        {
            if (!patterns_.ContainsKey(type))
                throw new KeyNotFoundException();
            this.SelectedType = type;
            this.orientation_ = orientation;
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

        /// <summary>
        /// return the pattern current orientation
        /// </summary>
        /// <returns></returns>
        public int GetOrientation()
        {
            return this.orientation_;
        }

        /// <summary>
        /// Try to merge two patterns. If is possible, the current pattern will be modified
        /// </summary>
        /// <param name="pattern">The pattern to merge with the object</param>
        /// <returns>Merge succeded or not</returns>
        public bool MergePatterns(Pattern pattern, bool temp)
        {
            var pair = new KeyValuePair<Type, Type>(this.SelectedType, pattern.SelectedType);

            if (!mergings_.ContainsKey(pair))
            {  
                pair = new KeyValuePair<Type, Type>(pattern.SelectedType, this.SelectedType);
                if (!mergings_.ContainsKey(pair))
                    return false;
            }
            //if (!temp)
            SelectedType = mergings_[pair];
            this.orientation_ = pattern.orientation_;
            if (this.orientation_ >= patterns_[this.SelectedType].Count)
                this.orientation_ = 0;
            return true;
        }
    }
}