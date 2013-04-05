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
using Android.Graphics.Drawables;

namespace Kaboom.Sources
{
    public class ElementMenu
    {
        public string Name
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public int Image
        {
            get;
            set;
        }
    }

    public class CustomListAdapter : BaseAdapter
    {
        Activity context;
        public List<ElementMenu> items;

        public CustomListAdapter(Activity context, List<ElementMenu> list)
            : base()
        {

            /*
            "TutoNormalBomb", "TutoLineBomb", "TutoConeBomb", "TutoXBomb", "TutoCheckPointBS",
            "TutoHBomb", "TutoUltimateBomb", "TutoBonusTNT", "A-Maze-Me", "CombisTheG", "Corporate",
            "ChooseYourSide", "DidUCheckTuto", "DynamiteWarehouse", "FaceToFace", "FindYourWayOut",
            "InTheRedCorner", "Invasion", "It's Something", "Life", "NumbaWan", "OneStepAway",
            "OppositeForces", "Tetris", "TheBreach", "Unreachable", "Versus", "XFactor"
            */


            this.context = context;
            this.items = list;
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];         
           
            var view = (convertView ?? 
                context.LayoutInflater.Inflate(
                    Resource.Layout.LevelSelector, 
                    parent, 
                    false)) as LinearLayout;
            var imageItem = view.FindViewById(Resource.Id.imageItem) as ImageView;
            var textTop = view.FindViewById(Resource.Id.textTop) as TextView;
            var textBottom = view.FindViewById(Resource.Id.textBottom) as TextView;
            imageItem.SetImageResource(item.Image);
            textTop.SetText(item.Name, TextView.BufferType.Normal);
            textBottom.SetText(item.Description, TextView.BufferType.Normal);
            return view;
        }
        public ElementMenu GetItemAtPosition(int position)
        {
            return items[position];
        }
    }
}
