using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Kaboom
{
    [Activity(Label = "Kaboom", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            MainGame.Activity = this;

            var g = new MainGame();
            SetContentView(g.Window);

            g.Run();
        }
    }
}

