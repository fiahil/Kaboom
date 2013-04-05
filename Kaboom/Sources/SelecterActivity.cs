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
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "TutoNormalBomb"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "TutoLineBomb"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "TutoConeBomb"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "TutoXBomb"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "TutoCheckPointBS"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "TutoHBomb"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "TutoUltimateBomb"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "TutoBonusTNT"}
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
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "A-Maze-Me"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "CombisTheG"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "Corporate"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "ChooseYourSide"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "DidUCheckTuto"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "DynamiteWarehouse"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "FaceToFace"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "FindYourWayOut"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "InTheRedCorner"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "Invasion"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "It's Something"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "Life"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "NumbaWan"},
                                new ElementMenu()
                                    {Description = "test", Image = Resource.Drawable.Icon, Name = "OneStepAway"}
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