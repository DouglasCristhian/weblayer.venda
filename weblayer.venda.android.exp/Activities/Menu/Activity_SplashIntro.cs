
using Android.App;
using Android.OS;
using Android.Views;

namespace weblayer.venda.android.exp.Activities
{
    [Activity(NoHistory = true, MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation |
    Android.Content.PM.ConfigChanges.ScreenSize)]

    public class Activity_SplashIntro : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.SplashLayout);

            if (Resources.Configuration.Orientation == Android.Content.Res.Orientation.Portrait)
            {
                this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            }
            else
            {
                this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
            }

            System.Threading.ThreadPool.QueueUserWorkItem(o => LoadActivity());
        }

        private void LoadActivity()
        {
            System.Threading.Thread.Sleep(2000); //Simulate a long pause    
            RunOnUiThread(() => StartActivity(typeof(Activity_Home)));
        }
    }
}