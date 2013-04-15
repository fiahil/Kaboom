using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Widget;
using System;


namespace Kaboom.Sources
{

    [Activity(Label = "Kaboom",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.SensorLandscape,
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation,
        Icon = "@drawable/icon")]
    public class MenuActivity : Activity
    {
        readonly List<string> mapName_;


        public MenuActivity()
        {
            mapName_ = new List<string>
                {
                    "A-Maze-Me",
                    "CombisTheG",
                    "Corporate",
                    "ChooseYourSide",
                    "DidUCheckTuto",
                    "DynamiteWarehouse",
                    "FaceToFace",
                    "FindYourWayOut",
                    "InTheRedCorner",
                    "Invasion",
                    "It's Something",
                    "Life",
                    "NumbaWan",
                    "OneStepAway",
                    "OppositeForces",
                    "Tetris",
                    "TheBreach",
                    "Unreachable",
                    "Versus",
                    "XFactor"
                };
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (AutoMediaPlayer.Instance == null)
                AutoMediaPlayer.Instance = MediaPlayer.Create(this, Resource.Raw.MenuAmbiance);

            SetContentView(Resource.Layout.ModeSelection);

            var quickButton = FindViewById<Button>(Resource.Id.QuickButton);
            quickButton.Click += (sender, e) =>
                {
                    var rand = new Random();
                    StartActivity(new Intent(this, typeof (MainActivity)).PutExtra("level",
                                                                                   mapName_[rand.Next(0, 19)]));
                };
            var mapButton = FindViewById<Button>(Resource.Id.MapButton);
            mapButton.Click += (sender, e) => StartActivity(new Intent(this, typeof (SelecterActivity)).PutExtra("type", "map"));
            var tutoButton = FindViewById<Button>(Resource.Id.TutoButton);
            tutoButton.Click += (sender, e) => StartActivity(new Intent(this, typeof (SelecterActivity)).PutExtra("type", "tuto"));
        }

        protected override void OnStart()
        {
            base.OnStart();
            AutoMediaPlayer.Instance.Start();
        }
        
        protected override void OnStop()
        {
            base.OnStop();
            AutoMediaPlayer.Instance.Pause();
        }

        protected override void OnPause()
        {
            base.OnPause();
            AutoMediaPlayer.Instance.Pause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            AutoMediaPlayer.Instance.Start();
        }
    }
}