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
    enum eVisibility
    {
        OPAQUE,
        TRANSPARENT
    }

    interface IEntity
    {
        int ZIndex { get; set; }
        eVisibility Visibility { get; set; }

        void Draw(SpriteBatch sb, GameTime t, Rectangle r);
    }

    class EntityComparer : IComparer<IEntity>
    {
        public int Compare(IEntity a, IEntity b)
        {
            return a.ZIndex - b.ZIndex;
        }
    }

    class UnitestEntity : IEntity
    {
        private int zindex_;
        private eVisibility visibility_;
        private Texture2D tile_;

        public UnitestEntity(int z, Texture2D tile, eVisibility v = eVisibility.OPAQUE)
        {
            this.zindex_ = z;
            this.visibility_ = v;
            this.tile_ = tile;
        }

        public eVisibility Visibility
        {
            get { return visibility_; }
            set { visibility_ = value; }
        }
        
        public int ZIndex
        {
            get { return zindex_; }
            set { zindex_ = value; }
        }

        public void Update(GameTime t)
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