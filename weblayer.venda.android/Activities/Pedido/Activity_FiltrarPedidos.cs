using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.android.Adapters;

namespace weblayer.venda.android.Activities
{
    [Activity(MainLauncher = true, Label = "")]
    public class Activity_FiltrarPedidos : Activity_Base
    {
        public CheckBox checkBoxOrcamento;
        public CheckBox checkBoxFinalizado;
        public CheckBox checkBoxSincronizado;
        public CheckBox checkBoxParcProcessado;
        public CheckBox checkBoxNaoProcessado;
        public CheckBox checkBoxCancelado;
        public CheckBox checkBoxParcFaturado;
        public CheckBox checkBoxFaturado;
        public CheckBox checkBoxParcEntregue;
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
            CheckBoxList();

            RestoreForm();

            spinnerDatas = PopulateSpinner();
            spinnerDataEmissao.Adapter = new ArrayAdapter<mSpinner>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, spinnerDatas);

            var prefs = Application.Context.GetSharedPreferences(MyPREFERENCES, FileCreationMode.WorldReadable);
            int resultado = prefs.GetInt("Id_DataEmissao", 0);
            spinnerDataEmissao.SetSelection(getIndexByValue(spinnerDataEmissao, resultado));
        }

        public void CheckBoxList()
        {
            lista = new CheckBox[10];
            lista[0] = checkBoxOrcamento;
            lista[1] = checkBoxFinalizado;
            lista[2] = checkBoxSincronizado;
            lista[3] = checkBoxParcProcessado;
            lista[4] = checkBoxNaoProcessado;
            lista[5] = checkBoxCancelado;
            lista[6] = checkBoxParcFaturado;
            lista[7] = checkBoxFaturado;
            lista[8] = checkBoxParcEntregue;
            lista[9] = checkBoxEntregue;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar_Filtro, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public void FindViews()
        {
            btnLimparFiltro = FindViewById<Button>(Resource.Id.btnFinalizar);
            spinnerDataEmissao = FindViewById<Spinner>(Resource.Id.spinnerDataEmissao);

            checkBoxOrcamento = FindViewById<CheckBox>(Resource.Id.checkBoxOrcamento);
            checkBoxFinalizado = FindViewById<CheckBox>(Resource.Id.checkBoxFinalizado);
            checkBoxSincronizado = FindViewById<CheckBox>(Resource.Id.checkBoxSincronizado);
            checkBoxParcProcessado = FindViewById<CheckBox>(Resource.Id.checkBoxParcProcessado);

            checkBoxNaoProcessado = FindViewById<CheckBox>(Resource.Id.checkBoxNaoProcessado);
            checkBoxCancelado = FindViewById<CheckBox>(Resource.Id.checkBoxCancelado);
            checkBoxParcFaturado = FindViewById<CheckBox>(Resource.Id.checkBoxParcFaturado);

            checkBoxFaturado = FindViewById<CheckBox>(Resource.Id.checkBoxFaturado);
            checkBoxParcEntregue = FindViewById<CheckBox>(Resource.Id.checkBoxParcEntregue);
            checkBoxEntregue = FindViewById<CheckBox>(Resource.Id.checkBoxEntregue);
        }

        public void BindData()
        {
            btnLimparFiltro.Click += BtnLimparFiltro_Click;
        }

        private void SetStyle()
        {
            spinnerDataEmissao.SetBackgroundResource(Resource.Drawable.EditTextStyle);

            if (checkBoxNaoProcessado.Checked)
            {
                checkBoxNaoProcessado.SetBackgroundResource(Resource.Drawable.CHECKBOX_CANCELADO1);
            }
        }

        private void BtnLimparFiltro_Click(object sender, System.EventArgs e)
        {
            checkBoxOrcamento.Checked = false;
            checkBoxFinalizado.Checked = false;
            checkBoxSincronizado.Checked = false;
            checkBoxParcProcessado.Checked = false;
            checkBoxNaoProcessado.Checked = false;
            checkBoxCancelado.Checked = false;
            checkBoxParcFaturado.Checked = false;
            checkBoxFaturado.Checked = false;
            checkBoxParcEntregue.Checked = false;
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

            #region Status
            string retornoCheckBox = "";
            if (checkBoxOrcamento.Checked)
                retornoCheckBox = retornoCheckBox + "0,";

            if (checkBoxFinalizado.Checked)
                retornoCheckBox = retornoCheckBox + "1,";

            if (checkBoxSincronizado.Checked)
                retornoCheckBox = retornoCheckBox + "2,";

            if (checkBoxParcProcessado.Checked)
                retornoCheckBox = retornoCheckBox + "3,";

            if (checkBoxNaoProcessado.Checked)
                retornoCheckBox = retornoCheckBox + "4,";

            if (checkBoxCancelado.Checked)
                retornoCheckBox = retornoCheckBox + "5,";

            if (checkBoxParcFaturado.Checked)
                retornoCheckBox = retornoCheckBox + "6,";

            if (checkBoxFaturado.Checked)
                retornoCheckBox = retornoCheckBox + "7,";

            if (checkBoxParcEntregue.Checked)
                retornoCheckBox = retornoCheckBox + "8,";

            if (checkBoxEntregue.Checked)
                retornoCheckBox = retornoCheckBox + "9,";
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

            Toast.MakeText(this, "Preferências de filtro atualizadas", ToastLength.Short).Show();

            Finish();
        }
    }
}
