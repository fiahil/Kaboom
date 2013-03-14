using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Kaboom.Serializer;

namespace KaboomEditor.Sources
{
    /// <summary>
    /// Store resources
    /// </summary>
    public class KeResources
    {
        public enum Type
        {
            Background,
            BlockUbk,
            BlockBk,
            Bomb,
            Checkpoint,
            Goal
        }

        public static Dictionary<Type, EntityProxy> Proxy = new Dictionary<Type, EntityProxy>
            {
                {
                    Type.Background, new EntityProxy
                        {
                            TileIdentifier = "background1",
                            TileFramePerAnim = new[] {1},
                            TileTotalAnim = 1,
                            TileFrameSpeed = 1,
                            ZIndex = 1
                        }
                },
                {
                    Type.BlockUbk, new BlockProxy
                        {
                            TileIdentifier = "background3",
                            TileFramePerAnim = new[] {1},
                            TileTotalAnim = 1,
                            TileFrameSpeed = 1,
                            Destroyable = false,
                            GameEnd = false
                        }
                },
                {
                    Type.BlockBk, new BlockProxy
                        {
                            TileIdentifier = "background2",
                            TileFramePerAnim = new[] {1, 2},
                            TileTotalAnim = 2,
                            TileFrameSpeed = 2,
                            Destroyable = true,
                            GameEnd = false
                        }
                },
                {
                    Type.Goal, new BlockProxy
                        {
                            TileIdentifier = "goal",
                            TileFramePerAnim = new[] {1, 1},
                            TileTotalAnim = 2,
                            TileFrameSpeed = 1,
                            Destroyable = true,
                            GameEnd = true
                        }
                },
                {
                    Type.Bomb, new BombProxy
                        {
                            Type = 1,
                            TileIdentifier = "BombSheetTNT",
                            TileFramePerAnim = new[] {1, 25},
                            TileTotalAnim = 2,
                            TileFrameSpeed = 20
                        }
                },
                {
                    Type.Checkpoint, new CheckPointProxy
                        {
                            TileIdentifier = "checkpoint",
                            TileFramePerAnim = new[] {1},
                            TileTotalAnim = 1,
                            TileFrameSpeed = 1,
                            Activated = false,
                            Bombsetidx = 1
                        }
                }
            };

        public static Dictionary<Type, BitmapImage> Bitmap = new Dictionary<Type, BitmapImage>
            {
                {Type.Background, new BitmapImage(new Uri(@"../../Resources/background1.png", UriKind.Relative))},
                {Type.BlockBk, new BitmapImage(new Uri(@"../../Resources/background2.png", UriKind.Relative))},
                {Type.BlockUbk, new BitmapImage(new Uri(@"../../Resources/background3.png", UriKind.Relative))},
                {Type.Bomb, new BitmapImage(new Uri(@"../../Resources/BombSheetTNT.png", UriKind.Relative))},
                {Type.Checkpoint, new BitmapImage(new Uri(@"../../Resources/Checkpoint.png", UriKind.Relative))},
                {Type.Goal, new BitmapImage(new Uri(@"../../Resources/GoalSheet.png", UriKind.Relative))}
            };

        public static Dictionary<Type, int> Index = new Dictionary<Type, int>
            {
                {Type.Background, 0},
                {Type.Checkpoint, 1},
                {Type.BlockBk, 2},
                {Type.BlockUbk, 2},
                {Type.Bomb, 2},
                {Type.Goal, 3}
            };

        public static readonly Dictionary<string, Type> TypeLink = new Dictionary<string, Type>
            {
                {"background1", Type.Background},
                {"background2", Type.BlockBk},
                {"background3", Type.BlockUbk},
                {"BombSheetTNT", Type.Bomb},
                {"goal", Type.Goal},
                {"checkpoint", Type.Checkpoint}
            };
    }
}
