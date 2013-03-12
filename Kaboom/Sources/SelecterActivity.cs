using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;

namespace Kaboom.Sources
{
    [Activity(Label = "Kaboom",
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.SensorLandscape,
        Icon = "@drawable/icon")]
    public class SelecterActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            FindViewById<ListView>(Resource.Id.listViewLevel).Adapter = new ArrayAdapter(this,
                                                                                         Android.Resource.Layout.SimpleListItem1,
                                                                                         new[] { "level1", "level2", "TutoNormalBomb", "TutoLineBomb", "TutoConeBomb", "TutoXBomb", "TutoCheckPointBS", "TutoHBomb", "TutoUltimateBomb", "TutoBonusTNT", "A-Maze-Me", "CombisTheG", "DidUCheckTuto", "TheBreach" });

            FindViewById<ListView>(Resource.Id.listViewLevel).ItemClick +=
                (sender, e) =>
                StartActivity(new Intent(this, typeof (MainActivity)).PutExtra("level", ((TextView) e.View).Text));
        }
    }
}