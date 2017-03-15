using Android.App;
using Android.Content;
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
        public CheckBox checkBoxOrcamento;
        public CheckBox checkBoxFinalizado;
        public CheckBox checkBoxFaturado;
        public CheckBox checkBoxEntregue;
        private Button btnLimparFiltro;
        private Spinner spinnerDataEmissao;
        private List<mSpinner> spinnerDatas;
        public string MyPREFERENCES = "MyPrefs";
        public CheckBox[] lista;

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

            lista = new CheckBox[4];
            lista[0] = checkBoxOrcamento;
            lista[1] = checkBoxFinalizado;
            lista[2] = checkBoxFaturado;
            lista[3] = checkBoxEntregue;

            RestoreForm();

            spinnerDatas = PopulateSpinner();
            spinnerDataEmissao.Adapter = new ArrayAdapter<mSpinner>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, spinnerDatas);

            var prefs = Application.Context.GetSharedPreferences(MyPREFERENCES, FileCreationMode.WorldReadable);
            int resultado = prefs.GetInt("Id_DataEmissao", 0);
            spinnerDataEmissao.SetSelection(getIndexByValue(spinnerDataEmissao, resultado));
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
                    SaveForm();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private List<mSpinner> PopulateSpinner()
        {
            List<mSpinner> minhalista = new List<mSpinner>();

            minhalista.Add(new mSpinner(0, "Todos pedidos emitidos"));
            minhalista.Add(new mSpinner(1, "Pedidos emitidos hoje"));
            minhalista.Add(new mSpinner(2, "Pedidos emitidos esta semana"));
            minhalista.Add(new mSpinner(3, "Pedidos emitidos este mês"));

            return minhalista;
        }

        private void RestoreForm()
        {
            var prefs = Application.Context.GetSharedPreferences(MyPREFERENCES, FileCreationMode.WorldReadable);

            int i = 0;
            foreach (CheckBox check in lista)
            {
                int result;
                var pref = Application.Context.GetSharedPreferences(MyPREFERENCES, FileCreationMode.WorldReadable);

                result = prefs.GetInt("CheckBox" + i.ToString(), -1);
                if (result == 0)
                {
                    check.Checked = true;
                }
                else
                {
                    check.Checked = false;
                }
                i++;
            }
        }

        private int getIndexByValue(Spinner spinner, long myId)
        {
            int index = 0;

            var adapter = (ArrayAdapter<mSpinner>)spinner.Adapter;
            for (int i = 0; i < spinner.Count; i++)
            {
                if (adapter.GetItemId(i) == myId)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private void SaveForm()
        {
            var prefs = Application.Context.GetSharedPreferences("MyPrefs", FileCreationMode.WorldWriteable);
            var prefEditor = prefs.Edit();

            int i = 0;
            foreach (CheckBox check in lista)
            {
                if (check.Checked == true)
                {
                    check.Id = i;
                    prefEditor.PutInt("CheckBox" + check.Id.ToString(), 0);
                    i++;
                }
                else
                {
                    check.Id = i;
                    prefEditor.PutInt("CheckBox" + check.Id.ToString(), -1);
                    i++;
                }
            }

            prefEditor.PutInt("Id_DataEmissao", spinnerDataEmissao.SelectedItemPosition);
            prefEditor.Commit();

            Toast.MakeText(this, "Preferências de filtro atualizadas", ToastLength.Short).Show();

            #region Status
            string retornoCheckBox = "";
            if (checkBoxOrcamento.Checked)
                retornoCheckBox = retornoCheckBox + "0,";

            if (checkBoxFinalizado.Checked)
                retornoCheckBox = retornoCheckBox + "1,";

            if (checkBoxFaturado.Checked)
                retornoCheckBox = retornoCheckBox + "2,";

            if (checkBoxEntregue.Checked)
                retornoCheckBox = retornoCheckBox + "3,";
            #endregion

            #region DataEmissao
            int retorno_Data = 0;
            if (spinnerDataEmissao.SelectedItemPosition == 1)
            {
                retorno_Data = 1;
            }
            else if (spinnerDataEmissao.SelectedItemPosition == 2)
            {
                retorno_Data = 2;
            }
            else if (spinnerDataEmissao.SelectedItemPosition == 3)
            {
                retorno_Data = 3;
            }
            //else
            //    retorno_Data = 0;
            #endregion

            Intent intent = new Intent();
            intent.PutExtra("Status", retornoCheckBox);
            intent.PutExtra("DataEmissao", retorno_Data);
            SetResult(Result.Ok, intent);

            Finish();
        }
    }
}