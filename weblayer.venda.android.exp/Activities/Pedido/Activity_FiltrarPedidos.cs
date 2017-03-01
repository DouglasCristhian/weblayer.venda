using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.android.exp.Activities;
using weblayer.venda.android.exp.Adapters;

namespace weblayer.venda.android.exp
{
    [Activity(MainLauncher = false, Label = "")]
    public class Activity_FiltrarPedidos : Activity_Base
    {
        private CheckBox checkBoxOrcamento;
        private CheckBox checkBoxFinalizado;
        private CheckBox checkBoxFaturado;
        private CheckBox checkBoxEntregue;
        private Button btnLimparFiltro;
        private Spinner spinnerDataEmissao;
        private List<mSpinner> spinnerDatas;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_FiltrarPedidos;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FindViews();
            BindData();
            SetStyle();

            spinnerDatas = PopulateSpinner();
            spinnerDataEmissao.Adapter = new ArrayAdapter<mSpinner>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, spinnerDatas);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar_Filtro, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public void FindViews()
        {
            btnLimparFiltro = FindViewById<Button>(Resource.Id.btnFinalizar);
            checkBoxOrcamento = FindViewById<CheckBox>(Resource.Id.checkBoxOrcamento);
            checkBoxFinalizado = FindViewById<CheckBox>(Resource.Id.checkBoxFinalizado);
            checkBoxFaturado = FindViewById<CheckBox>(Resource.Id.checkBoxFaturado);
            checkBoxEntregue = FindViewById<CheckBox>(Resource.Id.checkBoxEntregue);
            spinnerDataEmissao = FindViewById<Spinner>(Resource.Id.spinnerDataEmissao);
        }

        public void BindData()
        {
            btnLimparFiltro.Click += BtnLimparFiltro_Click;
        }

        private void SetStyle()
        {
            spinnerDataEmissao.SetBackgroundResource(Resource.Drawable.EditTextStyle);
        }

        private void BtnLimparFiltro_Click(object sender, System.EventArgs e)
        {
            checkBoxOrcamento.Checked = false;
            checkBoxFinalizado.Checked = false;
            checkBoxFaturado.Checked = false;
            checkBoxEntregue.Checked = false;
            spinnerDataEmissao.SetSelection(0);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_aceitar:
                    //SALVAR 
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private List<mSpinner> PopulateSpinner()
        {
            List<mSpinner> minhalista = new List<mSpinner>();

            minhalista.Add(new mSpinner(0, "Selecione o período..."));
            minhalista.Add(new mSpinner(1, "Pedidos emitidos hoje"));
            minhalista.Add(new mSpinner(2, "Pedidos emitidos esta semana"));
            minhalista.Add(new mSpinner(3, "Pedidos emitidos este mês"));

            return minhalista;
        }
    }
}