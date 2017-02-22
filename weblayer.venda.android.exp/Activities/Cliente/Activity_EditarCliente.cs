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
    [Activity(Label = "Cliente")]
    public class Activity_EditarCliente : Activity_Base
    {
        private string spinvalortbl;
        private EditText txtCodCli;
        private EditText txtRazaoSocialCli;
        private EditText txtNomeFantasiaCli;
        private EditText txtCNPJCli;
        public Spinner spinnerTabelaPreco;
        private Cliente cli;
        List<mSpinner> tblprecospinner;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_EditarClientes;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_salvar:
                    if (txtCNPJCli.Length() < 14)
                    {
                        txtCNPJCli.Error = "CNPJ inválido!";
                    }
                    else
                    {
                        Save();
                        return true;
                    }
                    break;

                case Resource.Id.action_deletar:
                    Delete();
                    return true;

            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var jsonnota = Intent.GetStringExtra("JsonNotaCli");

            if (jsonnota == null)
            {
                cli = null;
            }
            else
            {
                cli = Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(jsonnota);
            }

            FindViews();
            SetStyle();
            BindView();

            tblprecospinner = PopulateTabPrecoSpinnerList();
            spinnerTabelaPreco.Adapter = new ArrayAdapter<mSpinner>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, tblprecospinner);

            if (cli != null)
            {
                spinnerTabelaPreco.SetSelection(getIndexByValue(spinnerTabelaPreco, cli.id));
            }
            else
                cli = null;

            //BindData();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar, menu);
            menu.RemoveItem(Resource.Id.action_adicionar);
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_help);
            menu.RemoveItem(Resource.Id.action_sair);

            if (cli == null)
            {
                menu.RemoveItem(Resource.Id.action_deletar);
            }

            return base.OnCreateOptionsMenu(menu);
        }

        private void FindViews()
        {
            txtCodCli = FindViewById<EditText>(Resource.Id.txtCodigoCliente);
            txtNomeFantasiaCli = FindViewById<EditText>(Resource.Id.txtNomeFantasia);
            txtRazaoSocialCli = FindViewById<EditText>(Resource.Id.txtRazaoSocial);
            txtCNPJCli = FindViewById<EditText>(Resource.Id.txtCNPJ);
            spinnerTabelaPreco = FindViewById<Spinner>(Resource.Id.spinnerTblPrecos);

            txtCNPJCli.AddTextChangedListener(new Mask(txtCNPJCli, "##.###.###/####-##"));

            spinnerTabelaPreco.ItemSelected += new EventHandler<ItemSelectedEventArgs>(spinTblPreco_ItemSelected);
        }

        private void BindView()
        {
            if (cli == null)
                return;

            txtCodCli.Text = cli.id_Codigo;
            txtNomeFantasiaCli.Text = cli.ds_NomeFantasia;
            txtRazaoSocialCli.Text = cli.ds_RazaoSocial;
            txtCNPJCli.Text = cli.ds_Cnpj;
            spinvalortbl = cli.id_tabelapreco.ToString();
        }

        private void SetStyle()
        {
            txtCodCli.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtRazaoSocialCli.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtNomeFantasiaCli.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtCNPJCli.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            spinnerTabelaPreco.SetBackgroundResource(Resource.Drawable.EditTextStyle);
        }

        private void BindModel()
        {
            if (cli == null)
                cli = new Cliente();

            cli.id_Codigo = txtCodCli.Text;
            cli.ds_NomeFantasia = txtNomeFantasiaCli.Text;
            cli.ds_RazaoSocial = txtRazaoSocialCli.Text;
            cli.ds_Cnpj = txtCNPJCli.Text;
            var mytabelapreco = tblprecospinner[spinnerTabelaPreco.SelectedItemPosition];
            cli.id_tabelapreco = mytabelapreco.Id();
        }

        private bool ValidateViews()
        {
            var validacao = true;

            if (txtCodCli.Length() == 0)
            {
                validacao = false;
                txtCodCli.Error = "Código do cliente inválido!";
            }

            if (txtNomeFantasiaCli.Length() == 0)
            {
                validacao = false;
                txtNomeFantasiaCli.Error = "Nome fantasia inválido!";
            }

            if (txtRazaoSocialCli.Length() == 0)
            {
                validacao = false;
                txtRazaoSocialCli.Error = "Razão social inválida!";
            }

            if (txtCNPJCli.Length() == 0)
            {
                validacao = false;
                txtCNPJCli.Error = "CNPJ inválido!";
            }

            if (spinnerTabelaPreco.SelectedItemPosition == 0)
            {
                validacao = false;
                Toast.MakeText(this, "Por favor, insira a tabela de preços desejada!", ToastLength.Short).Show();
            }

            return validacao;
        }

        private void spinTblPreco_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            spinvalortbl = spinnerTabelaPreco.SelectedItem.ToString();
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

                var cliente = new Cliente_Manager();
                cliente.Save(cli);

                Intent myIntent = new Intent(this, typeof(Activity_Cliente));
                myIntent.PutExtra("mensagem", cliente.Mensagem);
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
                    var cliente = new Cliente_Manager();
                    cliente.Delete(cli);

                    Intent myIntent = new Intent(this, typeof(Activity_Cliente));
                    myIntent.PutExtra("mensagem", cliente.Mensagem);
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