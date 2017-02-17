using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using weblayer.venda.android.Adapters;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;
using static Android.Widget.AdapterView;

namespace weblayer.venda.android.Activities
{
    [Activity(Label = "Produto", MainLauncher = false)]
    public class Activity_EditarProduto : Activity_Base
    {
        private EditText txtCodigoProd;
        private EditText txtNomeProd;
        private EditText txtValorProd;
        private Spinner spinUniMedidaProd;
        private Produto prod;
        private string[] unidades_medida;
        private string spinValor;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_EditarProduto;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var jsonnota = Intent.GetStringExtra("JsonNotaProd");
            if (jsonnota == null)
            {
                prod = null;
            }
            else
            {
                prod = Newtonsoft.Json.JsonConvert.DeserializeObject<Produto>(jsonnota);
            }

            unidades_medida = new string[]
            {
                "Selecione", "CX", "PCT", "UN"
            };

            FindViews();
            SetStyle();
            BindView();
            BindData();

            spinUniMedidaProd.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, unidades_medida);

            if (prod != null)
            {
                spinUniMedidaProd.SetSelection(getIndex(spinUniMedidaProd, prod.ds_unimedida));
            }
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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar, menu);
            menu.RemoveItem(Resource.Id.action_salvar);
            menu.RemoveItem(Resource.Id.action_deletar);
            menu.RemoveItem(Resource.Id.action_adicionar);
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_refresh);
            menu.RemoveItem(Resource.Id.action_help);
            menu.RemoveItem(Resource.Id.action_sair);

            return base.OnCreateOptionsMenu(menu);
        }

        private void FindViews()
        {
            txtCodigoProd = FindViewById<EditText>(Resource.Id.txtCodigo);
            txtNomeProd = FindViewById<EditText>(Resource.Id.txtNome);
            txtValorProd = FindViewById<EditText>(Resource.Id.txtValorProd);
            spinUniMedidaProd = FindViewById<Spinner>(Resource.Id.spinnerUnidadeMedida);
            spinUniMedidaProd.ItemSelected += new EventHandler<ItemSelectedEventArgs>(spinUnidadeMedidadProd_ItemSelected);

        }

        private void SetStyle()
        {
            txtCodigoProd.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtNomeProd.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtValorProd.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            spinUniMedidaProd.SetBackgroundResource(Resource.Drawable.EditTextStyle);
        }

        private void BindView()
        {
            if (prod == null)
                return;

            txtCodigoProd.Text = prod.id_codigo;
            txtNomeProd.Text = prod.ds_nome;
            txtValorProd.Text = prod.vl_Lista.ToString("##,##0.00");
            spinValor = prod.ds_unimedida.ToString();
        }

        private void BindModel()
        {
            if (prod == null)
                prod = new Produto();

            prod.id_codigo = txtCodigoProd.Text;
            prod.ds_nome = txtNomeProd.Text;
            prod.ds_unimedida = spinValor.ToString();
            prod.vl_Lista = double.Parse(txtValorProd.Text.ToString());
        }

        private void BindData()
        {
            spinUniMedidaProd.Enabled = false;
            txtCodigoProd.Enabled = false;
            txtNomeProd.Enabled = false;
            txtValorProd.Enabled = false;
        }

        private bool ValidateViews()
        {
            var validacao = true;

            if (txtCodigoProd.Length() == 0)
            {
                validacao = false;
                txtCodigoProd.Error = "Código do produto inválido!";
            }

            if (txtNomeProd.Length() == 0)
            {
                validacao = false;
                txtNomeProd.Error = "Nome do produto inválido!";
            }

            if (spinUniMedidaProd.SelectedItemPosition == 0)
            {
                validacao = false;
                Toast.MakeText(this, "Por favor, insira a unidade de medida!", ToastLength.Short).Show();
            }

            return validacao;
        }

        private void spinUnidadeMedidadProd_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            spinValor = spinUniMedidaProd.SelectedItem.ToString();
        }

        private int getIndex(Spinner spinner, string myString)
        {
            int index = 0;

            for (int i = 0; i < spinner.Count; i++)
            {
                if (spinner.GetItemAtPosition(i).ToString().Equals(myString, StringComparison.InvariantCultureIgnoreCase))
                {
                    index = i;
                    break;
                }
            }
            return index;
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

        private void Save()
        {
            if (!ValidateViews())
                return;
            try
            {
                BindModel();

                var produto = new Produto_Manager();
                produto.Save(prod);

                Intent myIntent = new Intent(this, typeof(Activity_Produto));
                myIntent.PutExtra("mensagem", produto.Mensagem);
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

            alert.SetTitle("Tem certeza que deseja excluir este produto?");

            alert.SetNegativeButton("Não!", (senderAlert, args) =>
            {

            });

            alert.SetPositiveButton("Sim!", (senderAlert, args) =>
            {
                try
                {
                    var produto = new Produto_Manager();
                    produto.Delete(prod);

                    Intent myIntent = new Intent(this, typeof(Activity_Produto));
                    myIntent.PutExtra("mensagem", produto.Mensagem);
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