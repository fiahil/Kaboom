using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
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
                                    {Description = "A tutorial map to learn how to use normal bomb", Image = Resource.Drawable.easyIcon, Name = "TutoNormalBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use line bomb", Image = Resource.Drawable.easyIcon, Name = "TutoLineBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use cone bomb", Image = Resource.Drawable.easyIcon, Name = "TutoConeBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use \"X\" bomb", Image = Resource.Drawable.easyIcon, Name = "TutoXBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how usefull are the checkpoints", Image = Resource.Drawable.easyIcon, Name = "TutoCheckpointBS"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use \"H\" bomb", Image = Resource.Drawable.easyIcon, Name = "TutoHBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to use ultimate bomb", Image = Resource.Drawable.easyIcon, Name = "TutoUltimateBomb"},
                                new ElementMenu()
                                    {Description = "A tutorial map to learn how to interact with TNT bonus", Image = Resource.Drawable.easyIcon, Name = "TutoBonusTNT"}
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
                                    {Description = "The first one you need to play", Image = Resource.Drawable.easyIcon, Name = "NumbaWan"},
                               new ElementMenu()
                                    {Description = "This one will not bother you then", Image = Resource.Drawable.easyIcon, Name = "DidUCheckTuto"},
                                new ElementMenu()
                                    {Description = "Something", Image = Resource.Drawable.easyIcon, Name = "It's Something"},
                                 new ElementMenu()
                                    {Description = "He got a better defense ... But you have cookie", Image = Resource.Drawable.easyIcon, Name = "Versus"},
                               new ElementMenu()
                                    {Description = "Combinations of bombs is the key", Image = Resource.Drawable.easyIcon, Name = "CombisTheG"},
                                 new ElementMenu()
                                    {Description = "Get out of this boxing ring", Image = Resource.Drawable.easyIcon, Name = "InTheRedCorner"},
                                 new ElementMenu()
                                    {Description = "There is somewhere where the wall is thinner", Image = Resource.Drawable.easyIcon, Name = "TheBreach"},
                                new ElementMenu()
                                    {Description = "They can't stand each other", Image = Resource.Drawable.easyIcon, Name = "OppositeForces"},
                                  new ElementMenu()
                                    {Description = "get through the X", Image = Resource.Drawable.easyIcon, Name = "XFactor"},
                                new ElementMenu()
                                    {Description = "Right ot left ? Choose your path", Image = Resource.Drawable.iconList, Name = "ChooseYourSide"},
                                new ElementMenu()
                                    {Description = "TNT, TNT Everywhere",Image = Resource.Drawable.iconList,Name = "DynamiteWarehouse"},
                               new ElementMenu()
                                    {Description = "Don't go straight forward", Image = Resource.Drawable.iconList, Name = "FaceToFace"},
                                new ElementMenu()
                                    {Description = "Just one inch further", Image = Resource.Drawable.iconList, Name = "OneStepAway"},
                                 new ElementMenu()
                                    {Description = "Which path wil you take ?", Image = Resource.Drawable.iconList, Name = "FindYourWayOut"},
                             new ElementMenu()
                                    {Description = "{Epitech.}", Image = Resource.Drawable.iconList, Name = "Corporate"},
                                 new ElementMenu()
                                    {Description = "But you have the power force to do it", Image = Resource.Drawable.iconList, Name = "Unreachable"},
                               new ElementMenu()
                                    {Description = "There is something wrong with these patterns", Image = Resource.Drawable.iconList, Name = "Tetris"},
                                new ElementMenu()
                                    {Description = "Life is something that everyone should try at least once", Image = Resource.Drawable.hardIcon, Name = "Life"},
                                new ElementMenu()
                                    {Description = "Don't let them have your back", Image = Resource.Drawable.hardIcon, Name = "Invasion"},
                               new ElementMenu()
                                    {Description = "The exit must be near ... or not", Image = Resource.Drawable.hardIcon, Name = "A-Maze-Me"}
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