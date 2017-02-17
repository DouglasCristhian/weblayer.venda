using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;

namespace weblayer.venda.android.Fragments
{
    [Activity(Label = "Fragment_Legendas")]
    public class Fragment_Legendas : DialogFragment
    {
        private Button btnEntendi;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Fragment_Legendas, container, false);
            this.Dialog.SetCanceledOnTouchOutside(false);

            btnEntendi = view.FindViewById<Button>(Resource.Id.btnEntendi);
            btnEntendi.Click += BtnEntendi_Click;

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }


        private void BtnEntendi_Click(object sender, EventArgs e)
        {
            OnDismiss(this.Dialog);
        }
    }
}