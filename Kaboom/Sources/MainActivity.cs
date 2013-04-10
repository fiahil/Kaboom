using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Xna.Framework;

namespace Kaboom.Sources
{


    [Activity(MainLauncher = false,
        Icon = "@drawable/icon",
        Theme = "@style/Theme.Splash",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.SensorLandscape,
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation)]
    public class MainActivity : AndroidGameActivity
    {
        private MainGame game_;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Game.Activity = this;
           
                this.game_ = new MainGame(Intent.GetStringExtra("level"));
                SetContentView(this.game_.Window);
                this.game_.Run();

        }
    }
}
