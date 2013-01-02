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

        public UnitestEntity(int z, eVisibility v = eVisibility.OPAQUE)
        {
            this.zindex_ = z;
            this.visibility_ = v;
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
    }
}