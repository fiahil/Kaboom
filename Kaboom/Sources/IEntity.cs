using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Kaboom.Sources
{
    /// <summary>
    /// Define Entities' visibility
    /// </summary>
    enum EVisibility
    {
        Opaque,
        Transparent
    }

    /// <summary>
    /// Define a drawable Kaboom component
    /// </summary>
    interface IEntity
    {
        int ZIndex { get; set; }
        EVisibility Visibility { get; set; }

        /// <summary>
        /// Draw an entity on the screen
        /// </summary>
        /// <param name="sb">SpriteBatch used to draw textures</param>
        /// <param name="t">Game clock</param>
        /// <param name="r">Position position offset used to draw objects</param>
        void Draw(SpriteBatch sb, GameTime t, Rectangle r);

        void Update(GameTime time);
    }

    /// <summary>
    /// Comparer for SortedSet
    /// </summary>
    class EntityComparer : IComparer<IEntity>
    {
        /// <summary>
        /// Compare two entities by matching their Z-index
        /// </summary>
        /// <param name="a">First entity</param>
        /// <param name="b">Second entity</param>
        /// <returns></returns>
        public int Compare(IEntity a, IEntity b)
        {
            return a.ZIndex - b.ZIndex;
        }
    }

    /// <summary>
    /// Exemple of implementation
    /// </summary>
    class UnitestEntity : IEntity
    {
        private readonly Texture2D tile_;

        public UnitestEntity(int z, Texture2D tile, EVisibility v = EVisibility.Opaque)
        {
            this.ZIndex = z;
            this.Visibility = v;
            this.tile_ = tile;
        }

        public EVisibility Visibility { get; set; }

        public int ZIndex { get; set; }

        public void Update(GameTime time)
        {
        }

        public void Draw(SpriteBatch sb, GameTime t, Rectangle r)
        {
            // TODO: Do not draw outside screen.
            sb.Draw(this.tile_,
                new Rectangle(
                    (r.X * Camera.Instance.DimX) + Camera.Instance.OffX,
                    (r.Y * Camera.Instance.DimY) + Camera.Instance.OffY,
                    Camera.Instance.DimX,
                    Camera.Instance.DimY), Color.White);
        }
    }
}