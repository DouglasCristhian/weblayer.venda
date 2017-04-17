using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;

namespace weblayer.venda.android.Activities
{
    [Activity(Label = "Sobre")]
    public class Activity_Sobre : Activity_Base
    {
        private ListView listviewSobre;
        private List<string> mItems;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_Sobre;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FindViews();
            BindData();

        }

        private void FindViews()
        {
            listviewSobre = FindViewById<ListView>(Resource.Id.listviewSobre);
        }

        private void BindData()
        {
            mItems = new List<string>();

            mItems.Add("Novidades");
            mItems.Add("Versão\n" + GetVersion());

            listviewSobre.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, mItems);
            listviewSobre.ItemClick += ListviewSobre_ItemClick;
        }

        private void ListviewSobre_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Id == 0)
            {
                StartActivity(typeof(Activity_Novidades));
            }
        }

        private string GetVersion()
        {
            return Application.Context.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName;
        }
    }
}