using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using weblayer.venda.android.core.Helpers;

namespace weblayer.venda.android.exp.Fragments
{
    public class Fragment_Status : DialogFragment
    {
        public event EventHandler<DialogEventArgs> DialogClosed;
        private Button btnFaturado;
        private Button btnEntregue;
        private Button btnFinalizado;
        private View view;
        string Retorno;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            view = inflater.Inflate(Resource.Layout.Fragment_Status, container, false);


            FindViews();
            BindData();

            return view;
        }

        private void FindViews()
        {
            btnFaturado = view.FindViewById<Button>(Resource.Id.btnFaturado);
            btnEntregue = view.FindViewById<Button>(Resource.Id.btnEntregue);
            btnFinalizado = view.FindViewById<Button>(Resource.Id.btnFinalizado);
        }

        private void BindData()
        {
            btnFaturado.Click += BtnFaturado_Click;
            btnEntregue.Click += BtnEntregue_Click;
            btnFinalizado.Click += BtnFinalizado_Click;
        }

        private void BtnFinalizado_Click(object sender, EventArgs e)
        {
            Retorno = "Finalizado";
            Dismiss();
        }

        private void BtnFaturado_Click(object sender, EventArgs e)
        {
            Retorno = "Faturado";
            Dismiss();
        }

        private void BtnEntregue_Click(object sender, EventArgs e)
        {
            Retorno = "Entregue";
            Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            if (DialogClosed != null)
            {
                DialogClosed(this, new DialogEventArgs { ReturnValue = Retorno });
            }

        }
    }
}