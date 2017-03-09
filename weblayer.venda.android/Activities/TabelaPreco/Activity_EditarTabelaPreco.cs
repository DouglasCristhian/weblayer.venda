using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.Activities
{
    [Activity(Label = "Tabela de Preço")]
    public class Activity_EditarTabelaPreco : Activity_Base
    {
        private EditText txtCodTabelaPreco;
        private EditText txtDescricaoTabelaPreco;
        private EditText txtValorTabelaPreco;
        private EditText txtDescMaxTabelaPreco;
        private TabelaPreco tblPreco;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_EditarTabelaPreco;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var jsonnota = Intent.GetStringExtra("JsonNotaTabela");
            if (jsonnota == null)
            {
                tblPreco = null;
            }
            else
            {
                tblPreco = Newtonsoft.Json.JsonConvert.DeserializeObject<TabelaPreco>(jsonnota);
            }

            FindViews();
            SetStyle();
            BindViews();
            BindData();
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
            menu.RemoveItem(Resource.Id.action_filtrar);

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

        private void SetStyle()
        {
            txtCodTabelaPreco.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtDescricaoTabelaPreco.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtValorTabelaPreco.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtDescMaxTabelaPreco.SetBackgroundResource(Resource.Drawable.EditTextStyle);
        }

        private void FindViews()
        {
            txtCodTabelaPreco = FindViewById<EditText>(Resource.Id.txtCodigoTabelaPreco);
            txtDescricaoTabelaPreco = FindViewById<EditText>(Resource.Id.txtDescricaoTabelaPreco);
            txtValorTabelaPreco = FindViewById<EditText>(Resource.Id.txtValorTabelaPreco);
            txtDescMaxTabelaPreco = FindViewById<EditText>(Resource.Id.txtDescontoMaxTabelaPreco);
        }

        private void BindViews()
        {
            if (tblPreco == null)
                return;

            txtCodTabelaPreco.Text = tblPreco.id_codigo;
            txtDescricaoTabelaPreco.Text = tblPreco.ds_descricao;
            txtValorTabelaPreco.Text = tblPreco.vl_valor.ToString("##,##0.00");
            txtDescMaxTabelaPreco.Text = tblPreco.vl_descontomaximo.ToString("##,##0.00");
        }

        private void BindData()
        {
            txtCodTabelaPreco.Enabled = false;
            txtDescricaoTabelaPreco.Enabled = false;
            txtValorTabelaPreco.Enabled = false;
            txtDescMaxTabelaPreco.Enabled = false;
        }

        private void BindModel()
        {
            if (tblPreco == null)
                tblPreco = new TabelaPreco();

            tblPreco.id_codigo = txtCodTabelaPreco.Text;
            tblPreco.ds_descricao = txtDescricaoTabelaPreco.Text;
            tblPreco.vl_valor = double.Parse(txtValorTabelaPreco.Text);
            tblPreco.vl_descontomaximo = double.Parse(txtDescMaxTabelaPreco.Text);

        }

        private bool ValidateViews()
        {
            var validacao = true;

            if (txtCodTabelaPreco.Length() == 0)
            {
                validacao = false;
                txtCodTabelaPreco.Error = "Código da tabela inválido!";
            }

            if (txtDescricaoTabelaPreco.Length() == 0)
            {
                validacao = false;
                txtDescricaoTabelaPreco.Error = "Descrição da tabela inválida!";
            }

            if (txtValorTabelaPreco.Length() == 0)
            {
                validacao = false;
                txtValorTabelaPreco.Error = "Valor da tabela inválido!";
            }

            if (txtDescMaxTabelaPreco.Length() == 0)
            {
                validacao = false;
                txtDescMaxTabelaPreco.Error = "Desconto máximo inválido!";
            }

            return validacao;
        }

        private void Save()
        {
            if (!ValidateViews())
                return;

            try
            {
                BindModel();
                var tabelapreco = new TabelaPreco_Manager();
                tabelapreco.Save(tblPreco);

                Intent myIntent = new Intent(this, typeof(Activity_TabelaPreco));
                myIntent.PutExtra("mensagem", tabelapreco.Mensagem);
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
            alert.SetTitle("Tem certeza que deseja excluir esta tabela?");

            alert.SetNegativeButton("Não!", (senderAlert, args) =>
            {

            });

            alert.SetPositiveButton("Sim!", (senderAlert, args) =>
            {
                try
                {
                    var tabela = new TabelaPreco_Manager();
                    tabela.Delete(tblPreco);

                    Intent myIntent = new Intent(this, typeof(Activity_Cliente));
                    myIntent.PutExtra("mensagem", tabela.Mensagem);
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