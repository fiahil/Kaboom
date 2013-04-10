using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System;


namespace Kaboom.Sources
{
    
    [Activity(Label = "Kaboom",
        MainLauncher = false,
        ScreenOrientation = ScreenOrientation.SensorLandscape,
        Icon = "@drawable/icon")]
    public class SelecterActivity : Activity
    {

        CustomListAdapter listAdapter_;
        private List<ElementMenu> list_;
       
        public SelecterActivity()
        {
            list_ = null;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var type = Intent.GetStringExtra("type");

            SetContentView(Resource.Layout.Main);
  
            if (type == "tuto")
            {
                #region Tuto

                var title = FindViewById<TextView>(Resource.Id.textView1);
                title.Text = "Select a tutorial";

                list_ = new List<ElementMenu>()
                            {
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use normal bomb", Image = Resource.Drawable.iconList, Name = "TutoNormalBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use line bomb", Image = Resource.Drawable.iconList, Name = "TutoLineBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use cone bomb", Image = Resource.Drawable.iconList, Name = "TutoConeBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use \"X\" bomb", Image = Resource.Drawable.iconList, Name = "TutoXBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how usefull are the checkpoints", Image = Resource.Drawable.iconList, Name = "TutoCheckPointBS"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use \"H\" bomb", Image = Resource.Drawable.iconList, Name = "TutoHBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use ultimate bomb", Image = Resource.Drawable.iconList, Name = "TutoUltimateBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to interact with TNT bonus", Image = Resource.Drawable.iconList, Name = "TutoBonusTNT"}
                            };
                #endregion
            }
            else
            {
                #region Map

                var title = FindViewById<TextView>(Resource.Id.textView1);
                title.Text = "Select a map";

                list_ = new List<ElementMenu>()
                            {
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "A-Maze-Me"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "CombisTheG"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "Corporate"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "ChooseYourSide"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "DidUCheckTuto"},
                                new ElementMenu() 
                                    {Description = "test", Image = Resource.Drawable.iconList,  Name = "DynamiteWarehouse"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "FaceToFace"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "FindYourWayOut"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "InTheRedCorner"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "Invasion"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "It's Something"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "Life"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "NumbaWan"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "OneStepAway"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "OppositeForces"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "Tetris"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "TheBreach"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "Unreachable"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "Versus"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.iconList, Name = "XFactor"}
                            };

                #endregion
            }


            listAdapter_ = new CustomListAdapter(this, list_);
            var listView = FindViewById<ListView>(Resource.Id.listView);
            listView.Adapter = listAdapter_;
            listView.ItemClick += (sender, e) =>
                                  StartActivity(new Intent(this, typeof (MainActivity)).PutExtra("level",
                                                                                                 this.listAdapter_.
                                                                                                     GetItemAtPosition(
                                                                                                         e.Position).
                                                                                                     Name));
        }

    }
}