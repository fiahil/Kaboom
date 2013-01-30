using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.Xna.Framework;

namespace Kaboom.Sources
{
    [Activity(Label = "Kaboom",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.Orientation)]
    public class MainActivity : AndroidGameActivity
    {
        private MainGame game_;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Game.Activity = this;

            this.game_ = new MainGame();

            SetContentView(this.game_.Window);
            this.game_.Run();
        }
    }
}

