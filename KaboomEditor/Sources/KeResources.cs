using System.Collections.Generic;
using Kaboom.Serializer;

namespace KaboomEditor.Sources
{
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
                            TileIdentifier = "background2",
                            TileFramePerAnim = new[] {1, 2},
                            TileTotalAnim = 2,
                            TileFrameSpeed = 2,
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
    }
}
