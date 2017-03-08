using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;
using weblayer.venda.android.exp.Helpers;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.exp.Activities
{
    [Activity(Label = "Item do Pedido")]
    public class Activity_PedidoItem : Activity_Base
    {
        private EditText txtIdProduto;
        private TextView txtValorLista;
        private EditText txtValorVenda;
        private EditText txtQuantidadeItem;
        private EditText txtValorTotal;
        private TextView txtDesconto;
        private Button btnSalvarPedidoItem;
        private Button btnSalvarOutroPedItem;
        private Button btnLimparPedidoItem;
        private Button btnSalvarAtualizar;
        private Button btnSomar;
        private Button btnSubtrair;
        private Button btnExcluirPedidoItem;
        private PedidoItem ped_item;
        private Pedido pedido;
        private Produto produto;
        private Cliente cliente;
        private double go;
        private string Operacao;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_PedidoItem;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    break;
                    //return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Trazendo o obj do pedido da tela anterior
            string jsonPedido = Intent.GetStringExtra("JsonPedido");
            if (jsonPedido == null)
                return;

            pedido = Newtonsoft.Json.JsonConvert.DeserializeObject<Pedido>(jsonPedido);

            string jsonProdutosPedidoList = Intent.GetStringExtra("JsonPedidoItem");
            if (jsonProdutosPedidoList != null) //Edição
            {
                ped_item = Newtonsoft.Json.JsonConvert.DeserializeObject<PedidoItem>(jsonProdutosPedidoList);
                Operacao = "editar";
            }
            else
            {
                Operacao = "incluir";
            }

            string jsonCliente = Intent.GetStringExtra("JsonCliente");
            if (jsonCliente != null)
                cliente = Newtonsoft.Json.JsonConvert.DeserializeObject<Cliente>(jsonCliente);

            FindViews();
            SetStyle();
            BindViews();
            BindData();

        }

        private void FindViews()
        {
            txtIdProduto = FindViewById<EditText>(Resource.Id.txtIdProduto);
            txtValorLista = FindViewById<TextView>(Resource.Id.txtValorLista);
            txtValorVenda = FindViewById<EditText>(Resource.Id.txtValorVenda);
            txtDesconto = FindViewById<EditText>(Resource.Id.txtDesconto);
            txtQuantidadeItem = FindViewById<EditText>(Resource.Id.txtQuantidade);
            txtValorTotal = FindViewById<EditText>(Resource.Id.txtValorTotal);

            btnSalvarPedidoItem = FindViewById<Button>(Resource.Id.btnSalvar);
            btnSalvarOutroPedItem = FindViewById<Button>(Resource.Id.btnSalvarOutro);
            btnLimparPedidoItem = FindViewById<Button>(Resource.Id.btnLimparPedidoItem);
            btnSubtrair = FindViewById<Button>(Resource.Id.btnMenos);
            btnSomar = FindViewById<Button>(Resource.Id.btnMais);

            btnSalvarAtualizar = FindViewById<Button>(Resource.Id.btnSalvarAtualizar);
            btnExcluirPedidoItem = FindViewById<Button>(Resource.Id.btnExcluirPedidoItem);

            if (Operacao == "incluir")
            {
                btnSalvarAtualizar.Visibility = ViewStates.Gone;
                btnExcluirPedidoItem.Visibility = ViewStates.Gone;
            }
            else if (Operacao == "editar")
            {
                btnSalvarPedidoItem.Visibility = ViewStates.Gone;
                btnSalvarOutroPedItem.Visibility = ViewStates.Gone;
                btnLimparPedidoItem.Visibility = ViewStates.Gone;
            }
        }

        private void BindViews()
        {
            if (ped_item == null)
                return;

            txtIdProduto.Text = ped_item.ds_produto.ToString();
            txtQuantidadeItem.Text = ped_item.nr_quantidade.ToString();
            txtValorLista.Text = ped_item.vl_Lista.ToString("#,##0.00");
            txtValorVenda.Text = ped_item.vl_Venda.ToString("#,##0.00");
            txtDesconto.Text = ped_item.vl_Desconto.ToString("##,##0.00");

            double go = double.Parse(ped_item.nr_quantidade.ToString()) * double.Parse(ped_item.vl_Venda.ToString());
            txtValorTotal.Text = go.ToString("#,##0.00");
        }

        private void SetStyle()
        {
            txtIdProduto.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtValorLista.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtValorVenda.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtQuantidadeItem.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtValorTotal.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtDesconto.SetBackgroundResource(Resource.Drawable.EditTextStyle);
        }

        private void BindModel()
        {
            if (ped_item == null)
                ped_item = new PedidoItem();

            ped_item.id_pedido = pedido.id;
            ped_item.ds_produto = txtIdProduto.Text.ToString();

            if (Operacao == "editar")
            {
                ped_item.id_produto = ped_item.id_produto;
            }
            else
            {
                ped_item.id_produto = int.Parse(produto.id_codigo);
            }

            ped_item.nr_quantidade = int.Parse(txtQuantidadeItem.Text.ToString());
            ped_item.vl_Lista = double.Parse(txtValorLista.Text.ToString());
            ped_item.vl_Venda = double.Parse(txtValorVenda.Text.ToString());
            ped_item.vl_Desconto = double.Parse(txtDesconto.Text.ToString());
        }

        private void BindData()
        {
            btnSalvarPedidoItem.Click += BtnSalvarPedidoItem_Click;
            btnSalvarOutroPedItem.Click += BtnSalvarOutroPedItem_Click;
            btnSalvarAtualizar.Click += BtnSalvarAtualizar_Click;
            btnLimparPedidoItem.Click += BtnLimparPedidoItem_Click;
            btnExcluirPedidoItem.Click += BtnExcluirPedidoItem_Click;
            btnSomar.Click += BtnSomar_Click;
            btnSubtrair.Click += BtnSubtrair_Click;

            txtValorVenda.TextChanged += TxtValorVenda_TextChanged;
            txtQuantidadeItem.TextChanged += TxtQuantidadeItem_TextChanged;

            if (Operacao == "incluir")
            {
                if (pedido.fl_status == 0)
                {
                    txtIdProduto.Click += TxtIdProduto_Click;
                    txtValorVenda.Enabled = true;
                    txtQuantidadeItem.Enabled = true;
                }
            }
            else
            {
                txtIdProduto.Enabled = false;
            }

            if (pedido.fl_status != 0)
            {
                txtValorVenda.Focusable = false;
                txtValorVenda.Enabled = false;
                txtQuantidadeItem.Focusable = false;
                txtQuantidadeItem.Enabled = false;
                btnSomar.Enabled = false;
                btnSubtrair.Enabled = false;
                txtDesconto.Enabled = false;
                btnSalvarAtualizar.Visibility = ViewStates.Gone;
                btnExcluirPedidoItem.Visibility = ViewStates.Gone;
                txtValorTotal.Enabled = false;
            }

            txtDesconto.Text = "0,00";
            txtValorLista.Enabled = false;
            txtDesconto.Enabled = false;
            txtValorTotal.Enabled = false;
            txtValorVenda.AddTextChangedListener(new CurrencyConverterHelper(txtValorVenda));
        }

        private void BtnSubtrair_Click(object sender, EventArgs e)
        {
            int quant = int.Parse(txtQuantidadeItem.Text.ToString()) - 1;

            if ((quant < 0) || (txtQuantidadeItem.Text == ""))
            {
                quant = 0;
            }

            txtQuantidadeItem.Text = quant.ToString();
        }

        private void BtnSomar_Click(object sender, EventArgs e)
        {
            int quant = int.Parse(txtQuantidadeItem.Text.ToString()) + 1;
            txtQuantidadeItem.Text = quant.ToString();
        }

        private void Clean()
        {
            txtIdProduto.Text = "";
            txtQuantidadeItem.Text = "0";
            txtValorVenda.Text = "0,00";
            txtValorLista.Text = "0,00";
            txtDesconto.Text = "0,00";
        }

        private bool ValidateViews()
        {
            var validacao = true;

            if (txtIdProduto.Length() == 0)
            {
                validacao = false;
                txtIdProduto.Error = "Código do produto inválido!";
            }

            if ((txtQuantidadeItem.Length() == 0) || (txtQuantidadeItem.Text == "0"))
            {
                validacao = false;
                txtQuantidadeItem.Error = "Quantidade inválida!";
            }

            if ((txtValorVenda.Text == "0,00") || (txtValorVenda.Text == "0"))
            {
                validacao = false;
                txtValorVenda.Error = "Valor de venda inválido!";
            }

            return validacao;
        }

        private void TxtQuantidadeItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            double valorVenda = double.Parse(txtValorVenda.Text);

            if ((txtQuantidadeItem.Text != "") && (txtValorVenda.Text != ""))
            {
                if (txtQuantidadeItem.Text.Length == 0)
                {
                    txtQuantidadeItem.Text = "0";
                }
                go = (double.Parse(valorVenda.ToString()) * double.Parse(txtQuantidadeItem.Text.ToString()));
            }
            else
            {
                go = 0;
            }

            txtValorTotal.Text = go.ToString("#,##0.00");
        }

        private void TxtValorVenda_TextChanged(object sender, TextChangedEventArgs e)
        {
            double desconto;

            if (txtValorVenda.Text == "" && txtIdProduto.Text == "")
            {
                txtValorVenda.Text = "0";
            }

            if (txtQuantidadeItem.Text == "")
            {
                txtValorVenda.Text = "0";
            }

            double valorVenda = double.Parse(txtValorVenda.Text.ToString());
            double valorLista = double.Parse(txtValorLista.Text.ToString());

            if ((txtValorVenda.Text.Length == 0) || (txtValorVenda.Text.ToString()) == "")
            {
                go = 0;
                desconto = 0;
                txtDesconto.Text = desconto.ToString("#,##0.00");
            }
            else if (double.Parse(valorVenda.ToString()) > double.Parse(valorLista.ToString()))
            {
                if (txtQuantidadeItem.Text.Length == 0)
                {
                    txtQuantidadeItem.Text = "0";
                }

                go = (double.Parse(valorVenda.ToString()) * double.Parse(txtQuantidadeItem.Text.ToString()));
                desconto = 0;
                txtDesconto.Text = desconto.ToString("#,##0.00");
            }
            else if (double.Parse(valorVenda.ToString()) < double.Parse(valorLista.ToString()))
            {
                if (txtQuantidadeItem.Text.Length == 0)
                {
                    txtQuantidadeItem.Text = "0";
                }
                go = (double.Parse(valorVenda.ToString()) * double.Parse(txtQuantidadeItem.Text.ToString()));
                desconto = (double.Parse(valorLista.ToString()) - double.Parse(valorVenda.ToString()));
                txtDesconto.Text = desconto.ToString("#,##0.00");
            }
            else
            {
                go = (double.Parse(valorVenda.ToString()) * double.Parse(txtQuantidadeItem.Text.ToString()));
            }
            txtValorTotal.Text = go.ToString("#,##0.00");
        }

        private void TxtIdProduto_Click(object sender, System.EventArgs e)
        {
            //Intent para pegar o produto escolhido e trazer para a activity PedidoItem
            Intent intent = new Intent();
            intent.SetClass(this, typeof(Activity_PedidoProduto));
            StartActivityForResult(intent, 0);
        }

        private void BtnSalvarAtualizar_Click(object sender, EventArgs e)
        {
            if (!ValidateViews())
                return;

            Save();
            Finish();
        }

        private void BtnLimparPedidoItem_Click(object sender, EventArgs e)
        {
            Clean();
        }

        private void BtnCancelarPedidoItem_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void BtnSalvarOutroPedItem_Click(object sender, EventArgs e)
        {
            if (!ValidateViews())
                return;
            try
            {
                Save();
                ped_item = null;

                Toast.MakeText(this, $"Item {txtIdProduto.Text} adicionado ao pedido com sucesso!", ToastLength.Long).Show();
                Intent intent = new Intent();
                SetResult(Result.Ok, intent);

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }

            Clean();
        }

        private void BtnSalvarPedidoItem_Click(object sender, EventArgs e)
        {
            if (!ValidateViews())
                return;
            try
            {
                Save();
                if (ValidateViews())
                {
                    Finish();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
            }
        }

        private void BtnExcluirPedidoItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                var jsonidproduto = data.GetStringExtra("JsonIdProduto");
                produto = Newtonsoft.Json.JsonConvert.DeserializeObject<Produto>(jsonidproduto);
                txtIdProduto.Text = produto.ds_nome;
                txtValorVenda.Enabled = true;

                var tabprecoprod = new ProdutoTabelaPreco_Manager().Get(cliente.id_tabelapreco, produto.id);

                if (tabprecoprod != null)
                {
                    txtValorLista.Text = tabprecoprod.vl_Valor.ToString("#,##0.00");
                    txtValorVenda.Text = tabprecoprod.vl_Valor.ToString("#,##0.00");
                    txtDesconto.Text = "0,00";
                }
                else
                {
                    txtValorLista.Text = produto.vl_Lista.ToString("#,##0.00");
                    txtValorVenda.Text = produto.vl_Lista.ToString("#,##0.00");
                }
            }
        }

        private void Save()
        {
            if (!ValidateViews())
                return;

            try
            {
                BindModel();

                var ped = new PedidoItem_Manager();
                ped.Save(ped_item);

                Intent myIntent = new Intent();
                myIntent.PutExtra("mensagem", ped.Mensagem);
                SetResult(Result.Ok, myIntent);
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
                    var ped = new PedidoItem_Manager();
                    ped.Delete(ped_item);

                    Intent intent = new Intent();
                    intent.PutExtra("mensagem", ped.Mensagem);
                    SetResult(Result.Ok, intent);

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