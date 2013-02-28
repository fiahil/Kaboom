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
            Bomb
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
                            Destroyable = false
                        }
                },
                {
                    Type.BlockBk, new BlockProxy
                        {
                            TileIdentifier = "background2",
                            TileFramePerAnim = new[] {1, 2},
                            TileTotalAnim = 2,
                            TileFrameSpeed = 2,
                            Destroyable = true
                        }
                },
                {
                    Type.Bomb, new BombProxy
                        {
                            Type = 0,
                            TileIdentifier = "BombSheet",
                            TileFramePerAnim = new[] {8, 18},
                            TileTotalAnim = 2,
                            TileFrameSpeed = 20
                        }
                }
            };

        public static Dictionary<Type, BitmapImage> Bitmap = new Dictionary<Type, BitmapImage>
            {
                {Type.Background, new BitmapImage(new Uri(@"../../Resources/background1.png", UriKind.Relative))},
                {Type.BlockBk, new BitmapImage(new Uri(@"../../Resources/background2.png", UriKind.Relative))},
                {Type.BlockUbk, new BitmapImage(new Uri(@"../../Resources/background3.png", UriKind.Relative))},
                {Type.Bomb, new BitmapImage(new Uri(@"../../Resources/Bomb.png", UriKind.Relative))}
            };
    }
}
