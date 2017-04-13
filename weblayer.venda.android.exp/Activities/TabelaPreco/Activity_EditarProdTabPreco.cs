using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using weblayer.venda.android.exp.Adapters;
using weblayer.venda.android.exp.Helpers;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;
using static Android.Widget.AdapterView;

namespace weblayer.venda.android.exp.Activities
{
    [Activity(Label = "Tabela de Preço do Produto", MainLauncher = false)]
    public class Activity_EditarProdTabelaPreco : Activity_Base
    {
        private string valoridproduto;
        private string valoridtabpreco;
        private EditText txt_ProdTabPreco;
        private Spinner spinIdProduto;
        private Spinner spinIdTabPreco;
        private ProdutoTabelaPreco prodtabpreco;
        List<mSpinner> tblprecospinner;
        List<mSpinner> tblprodutospinner;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_EditarProdTabelaPreco;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var jsonnota = Intent.GetStringExtra("JsonProdTblPreco");

            if (jsonnota == null)
            {
                prodtabpreco = null;
            }
            else
            {
                prodtabpreco = Newtonsoft.Json.JsonConvert.DeserializeObject<ProdutoTabelaPreco>(jsonnota);
            }

            FindView();
            SetStyle();
            BindView();

            tblprecospinner = PopulateTabPrecoSpinnerList();
            spinIdTabPreco.Adapter = new ArrayAdapter<mSpinner>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, tblprecospinner);

            tblprodutospinner = PopulateProdutoSpinnerList();
            spinIdProduto.Adapter = new ArrayAdapter<mSpinner>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, tblprodutospinner);

            if (prodtabpreco != null)
            {
                spinIdTabPreco.SetSelection(getIndexByValue(spinIdTabPreco, prodtabpreco.id_tabpreco));
                spinIdProduto.SetSelection(getIndexByValue(spinIdProduto, prodtabpreco.id_produto));
            }
            else
                prodtabpreco = null;

            txt_ProdTabPreco.AddTextChangedListener(new CurrencyConverterHelper(txt_ProdTabPreco));

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar, menu);
            menu.RemoveItem(Resource.Id.action_adicionar);
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_help);
            menu.RemoveItem(Resource.Id.action_sair);
            menu.RemoveItem(Resource.Id.action_filtrar);
            menu.RemoveItem(Resource.Id.action_legenda);

            if (prodtabpreco == null)
            {
                menu.RemoveItem(Resource.Id.action_deletar);
            }

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_salvar:
                    Save();
                    return true;

                case Resource.Id.action_deletar:
                    Delete();
                    return true;

            }
            return base.OnOptionsItemSelected(item);
        }

        private void FindView()
        {
            spinIdProduto = FindViewById<Spinner>(Resource.Id.spinnerIdProdutoTbl);
            spinIdTabPreco = FindViewById<Spinner>(Resource.Id.spinnerIdTabPreco);
            txt_ProdTabPreco = FindViewById<EditText>(Resource.Id.txtValorTabProd);

            spinIdTabPreco.ItemSelected += new EventHandler<ItemSelectedEventArgs>(spinIdTabPreco_ItemSelected);
            spinIdProduto.ItemSelected += new EventHandler<ItemSelectedEventArgs>(spinIdProduto_ItemSelected);
        }

        private void BindView()
        {
            if (prodtabpreco == null)
                return;

            valoridtabpreco = prodtabpreco.id_tabpreco.ToString();
            valoridproduto = prodtabpreco.id_produto.ToString();
            txt_ProdTabPreco.Text = prodtabpreco.vl_Valor.ToString("##,##0.00");
        }

        private void SetStyle()
        {
            txt_ProdTabPreco.SetBackgroundResource(Resource.Drawable.EditTextStyle);
        }

        private void BindModel()
        {
            if (prodtabpreco == null)
                prodtabpreco = new ProdutoTabelaPreco();

            var myproduto = tblprodutospinner[spinIdProduto.SelectedItemPosition];
            prodtabpreco.id_produto = myproduto.Id();
            var mytabpreco = tblprecospinner[spinIdTabPreco.SelectedItemPosition];
            prodtabpreco.id_tabpreco = mytabpreco.Id();
            prodtabpreco.vl_Valor = double.Parse(txt_ProdTabPreco.Text.ToString());
        }

        private bool ValidateViews()
        {
            var validacao = true;

            if (txt_ProdTabPreco.Length() == 0)
            {
                txt_ProdTabPreco.Error = "Valor inválido!";
            }

            if (spinIdTabPreco.SelectedItemPosition == 0)
            {
                validacao = false;
                Toast.MakeText(this, "Por favor, insira a tabela de preço!", ToastLength.Short).Show();
            }

            if (spinIdProduto.SelectedItemPosition == 0)
            {
                validacao = false;
                Toast.MakeText(this, "Por favor, insira o código do produto!", ToastLength.Short).Show();
            }


            return validacao;
        }

        private void spinIdProduto_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            valoridproduto = spinIdProduto.SelectedItem.ToString();
        }

        private void spinIdTabPreco_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            valoridtabpreco = spinIdTabPreco.SelectedItem.ToString();
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

        private List<mSpinner> PopulateTabPrecoSpinnerList()
        {
            List<mSpinner> minhalista = new List<mSpinner>();
            var listatabelapreco = new TabelaPrecoRepository().List();

            minhalista.Add(new mSpinner(0, "Selecione..."));

            foreach (var item in listatabelapreco)
            {
                minhalista.Add(new mSpinner(item.id, item.ds_descricao));
            }

            return minhalista;
        }

        private List<mSpinner> PopulateProdutoSpinnerList()
        {
            List<mSpinner> minhalista = new List<mSpinner>();
            var listaprodutos = new ProdutoRepository().List();

            minhalista.Add(new mSpinner(0, "Selecione..."));

            foreach (var item in listaprodutos)
            {
                minhalista.Add(new mSpinner(item.id, item.ds_nome));
            }

            return minhalista;
        }

        private void Save()
        {
            if (!ValidateViews())
                return;

            try
            {
                BindModel();

                var precos = new ProdutoTabelaPreco_Manager();
                precos.Save(prodtabpreco);

                Intent myIntent = new Intent(this, typeof(Activity_ProdTabPreco));
                myIntent.PutExtra("mensagem", precos.Mensagem);
                SetResult(Result.Ok, myIntent);
                Finish();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void Delete()
        {

            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Tem certeza que deseja excluir este cliente?");

            alert.SetNegativeButton("Não!", (senderAlert, args) =>
            {

            });

            alert.SetPositiveButton("Sim!", (senderAlert, args) =>
            {
                try
                {
                    var precos = new ProdutoTabelaPreco_Manager();
                    precos.Delete(prodtabpreco);

                    Intent myIntent = new Intent(this, typeof(Activity_Cliente));
                    myIntent.PutExtra("mensagem", precos.Mensagem);
                    SetResult(Result.Ok, myIntent);

                    Finish();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                }

            });

            RunOnUiThread(() =>
            {
                alert.Show();
            });
        }
    }
}