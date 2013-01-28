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
    [Activity(Label = "Kaboom", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AndroidGameActivity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MainGame.Activity = this;

            Game.Activity = this;

            var g = new MainGame();
            SetContentView(g.Window);

            g.Run();
        }
    }
}

