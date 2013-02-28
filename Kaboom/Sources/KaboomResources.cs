using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Kaboom.Serializer;

namespace Kaboom.Sources
{
    /// <summary>
    /// Static heavy resources 
    /// </summary>
    class KaboomResources
    {
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SpriteSheet> Sprites = new Dictionary<string, SpriteSheet>();
        public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public static Dictionary<string, MapElements> Levels = new Dictionary<string, MapElements>();
    }
}
