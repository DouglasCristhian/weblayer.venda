using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using weblayer.venda.android.core.Helpers;
using weblayer.venda.android.exp.Adapters;
using weblayer.venda.android.exp.Helpers;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;
using Uri = Android.Net.Uri;

namespace weblayer.venda.android.exp.Activities
{
    [Activity(Label = "Pedido")]
    public class Activity_EditarPedido : Activity_Base
    {
        private EditText txtid_Codigo;
        private EditText txtDataEmissao;
        private TextView txtValor_Total;
        private TextView txtStatusPedido;
        private EditText txtMsgPedido;
        private EditText txtMsgNF;
        private TextView lblStatusPedido;
        private Button btnAdicionar;
        private Button btnItensPedido;
        private Button btnGerarPDF;
        private Button btnFinalizar; 
        private Pedido pedido;
        private string idcliente;
        private Spinner spinnerClientes;
        List<mSpinner> tblclientespinner;
        private bool PROSSEGUIR;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_EditarPedidos;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_salvar:
                    Save();
                    if (ValidateViews())
                        Finish();
                    return true;

                case Resource.Id.action_deletar:
                    Delete();
                    return true;

            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var jsonnota = Intent.GetStringExtra("JsonPedido");
            if (jsonnota == null)
            {
                pedido = null;
            }
            else
            {
                pedido = Newtonsoft.Json.JsonConvert.DeserializeObject<Pedido>(jsonnota);
            }

            FindViews();
            SetStyle();
            BindData();
            BindViews();

            tblclientespinner = PopulateSpinner();
            spinnerClientes.Adapter = new ArrayAdapter<mSpinner>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, tblclientespinner);

            if (pedido != null)
            {
                spinnerClientes.SetSelection(getIndexByValue(spinnerClientes, pedido.id_cliente));
            }
            else
                pedido = null;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar, menu);
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_adicionar);
            menu.RemoveItem(Resource.Id.action_help);
            menu.RemoveItem(Resource.Id.action_sair);
            menu.RemoveItem(Resource.Id.action_filtrar);
            menu.RemoveItem(Resource.Id.action_legenda);
            menu.RemoveItem(Resource.Id.action_contato);

            if (pedido == null)
            {
                menu.RemoveItem(Resource.Id.action_deletar);
            }

            if (pedido != null)
            {
                if (pedido.fl_status != 0)
                {
                    menu.RemoveItem(Resource.Id.action_salvar);
                }
            }

            return base.OnCreateOptionsMenu(menu);
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

        private void FindViews()
        {
            txtid_Codigo = FindViewById<EditText>(Resource.Id.txtCodigoPedido);
            spinnerClientes = FindViewById<Spinner>(Resource.Id.spinnerIdCliente);
            txtDataEmissao = FindViewById<EditText>(Resource.Id.txtDataEmissao);
            txtValor_Total = FindViewById<TextView>(Resource.Id.txtValorTotal);
            txtMsgPedido = FindViewById<EditText>(Resource.Id.txtMsgPedido);
            txtMsgNF = FindViewById<EditText>(Resource.Id.txtMsgNF);
            lblStatusPedido = FindViewById<TextView>(Resource.Id.lblStatusPedido);
            txtStatusPedido = FindViewById<TextView>(Resource.Id.txtStatusPedido);
            btnAdicionar = FindViewById<Button>(Resource.Id.btnAdicionar);
            btnItensPedido = FindViewById<Button>(Resource.Id.btnItensPedido);
            btnGerarPDF = FindViewById<Button>(Resource.Id.btnGerarPDF);
            btnFinalizar = FindViewById<Button>(Resource.Id.btnFinalizar);

            txtDataEmissao.SetBackgroundColor(Android.Graphics.Color.LightGray);

            spinnerClientes.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(TblClientes_ItemSelected);
        }

        private void SetStyle()
        {
            txtid_Codigo.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtDataEmissao.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtValor_Total.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtMsgPedido.SetBackgroundResource(Resource.Drawable.EditTextStyle);
            txtMsgNF.SetBackgroundResource(Resource.Drawable.EditTextStyle);

            if (pedido != null)
            {
                if (pedido.fl_status == 0)
                    txtStatusPedido.SetBackgroundResource(Resource.Drawable.Status_Orcamento);

                if (pedido.fl_status == 1)
                    txtStatusPedido.SetBackgroundResource(Resource.Drawable.StatusFinalizado);

                if (pedido.fl_status == 2)
                    txtStatusPedido.SetBackgroundResource(Resource.Drawable.StatusFaturado);

                if (pedido.fl_status == 3)
                    txtStatusPedido.SetBackgroundResource(Resource.Drawable.StatusEntregue);
            }
        }

        private void TblClientes_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idcliente = spinnerClientes.SelectedItem.ToString();
        }

        private void BindViews()
        {
            if (pedido == null)
                return;

            txtid_Codigo.Text = pedido.id_codigo.ToString();
            idcliente = pedido.ds_cliente.ToString();
            txtValor_Total.Text = pedido.vl_total.ToString(("##,##0.00"));
            txtDataEmissao.Text = pedido.dt_emissao.Value.ToString("dd/MM/yyyy");
            txtMsgPedido.Text = pedido.ds_MsgPedido.ToString();
            txtMsgNF.Text = pedido.ds_MsgNF.ToString();
            txtStatusPedido.Text = Status();
        }

        private string Status()
        {
            string status = "";

            if (pedido.fl_status == 0)
            {
                status = "Or�amento";
            }

            if (pedido.fl_status == 1)
            {
                status = "Pedido Finalizado";
            }

            if (pedido.fl_status == 2)
            {
                status = "Faturado";
            }

            if (pedido.fl_status == 3)
            {
                status = "Entregue";
            }

            return status;
        }

        private void BindModel()
        {
            if (pedido == null)
                pedido = new Pedido();

            string data = (txtDataEmissao.Text);
            var datahora = DateTime.Parse(data, CultureInfo.CreateSpecificCulture("pt-BR"));

            pedido.id_codigo = txtid_Codigo.Text;
            pedido.id_vendedor = 1;
            pedido.ds_cliente = idcliente;
            var idcli = tblclientespinner[spinnerClientes.SelectedItemPosition];
            pedido.id_cliente = idcli.Id();
            pedido.dt_emissao = datahora;
            pedido.ds_MsgNF = txtMsgNF.Text.ToString();
            pedido.ds_MsgPedido = txtMsgPedido.Text.ToString();
        }

        private void BindData()
        {
            txtStatusPedido.Enabled = false;

            if (pedido == null)
            {
                txtDataEmissao.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDataEmissao.Click += EventtxtDataEmissao_Click;
                btnGerarPDF.Visibility = ViewStates.Gone;
                txtStatusPedido.Visibility = ViewStates.Gone;
                lblStatusPedido.Visibility = ViewStates.Gone;

                if (txtValor_Total.Text == "0")
                {
                    btnFinalizar.Visibility = ViewStates.Gone;
                    btnItensPedido.Visibility = ViewStates.Gone;
                    spinnerClientes.Enabled = true;
                }

                if (txtValor_Total.Text != "0")
                {
                    txtid_Codigo.Enabled = false;
                }
            }

            if (pedido != null)
            {
                if (pedido.fl_status != 0)
                {
                    txtid_Codigo.Enabled = false;
                    spinnerClientes.Enabled = false;
                    txtDataEmissao.Enabled = false;
                    txtMsgNF.Enabled = false;
                    txtMsgPedido.Enabled = false;
                    btnFinalizar.Visibility = ViewStates.Gone;
                    btnAdicionar.Visibility = ViewStates.Gone;
                }

                if ((pedido.vl_total == 0) || (txtValor_Total.Text == "0,00"))
                {
                    btnFinalizar.Visibility = ViewStates.Gone;
                    btnItensPedido.Visibility = ViewStates.Gone;
                    spinnerClientes.Enabled = true;
                    btnGerarPDF.Visibility = ViewStates.Gone;
                }
                else if ((pedido.vl_total != 0) || (txtValor_Total.Text != "0,00"))
                {
                    spinnerClientes.Enabled = false;
                    btnGerarPDF.Enabled = true;
                    btnGerarPDF.Visibility = ViewStates.Visible;
                }
            }

            txtValor_Total.Enabled = false;
            btnAdicionar.Click += BtnAdicionar_Click;
            btnFinalizar.Click += BtnFinalizar_Click;
            btnGerarPDF.Click += BtnGerarPDF_Click;
            txtValor_Total.Click += TxtValor_Total_Click;
            btnItensPedido.Click += TxtValor_Total_Click;

        }

        private bool ValidateViews()
        {
            var validacao = true;

            if (spinnerClientes.SelectedItemPosition == 0)
            {
                validacao = false;
                Toast.MakeText(this, "Por favor, selecione o c�digo do cliente", ToastLength.Short).Show();
            }

            return validacao;
        }

        private List<mSpinner> PopulateSpinner()
        {
            List<mSpinner> minhalista = new List<mSpinner>();
            var listaclientes = new ClienteRepository().List();

            minhalista.Add(new mSpinner(0, "Selecione o cliente..."));

            foreach (var item in listaclientes)
            {
                minhalista.Add(new mSpinner(item.id, item.ds_NomeFantasia));
            }

            return minhalista;
        }

        private void TxtValor_Total_Click(object sender, EventArgs e)
        {
            if (txtValor_Total.Text.ToString() == "0")
                return;

            Intent intent = new Intent();
            intent.SetClass(this, typeof(Activity_ProdutoPedidoList));
            intent.PutExtra("JsonPedido", Newtonsoft.Json.JsonConvert.SerializeObject(pedido));
            StartActivityForResult(intent, 0);
        }

        private void EventtxtDataEmissao_Click(object sender, EventArgs e)
        {
            DatePickerHelper frag = DatePickerHelper.NewInstance(delegate (DateTime time)
            {
                txtDataEmissao.Text = time.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"));
            });

            frag.Show(FragmentManager, DatePickerHelper.TAG);
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
        {
            if (!ValidateViews())
                return;

            //Come�a intent para adicionar um novo pedidoitem. Aguarda resultado para trazer o valor do item de volta
            Save();

            var obj_cliente = new Cliente_Manager().Get(pedido.id_cliente);

            Intent intent = new Intent();
            intent.SetClass(this, typeof(Activity_PedidoItem));
            intent.PutExtra("JsonPedido", Newtonsoft.Json.JsonConvert.SerializeObject(pedido));
            intent.PutExtra("JsonCliente", Newtonsoft.Json.JsonConvert.SerializeObject(obj_cliente));
            StartActivityForResult(intent, 0);
        }

        private void BtnGerarPDF_Click(object sender, EventArgs e)
        {
            PermissoesGarantidas();
            if (PROSSEGUIR == true)
            {
                GerarPDF();
            }

            //Java.IO.File file = new Java.IO.File(path);
            //Intent intent = new Intent(Intent.ActionView);
            //intent.SetDataAndType(Android.Net.Uri.FromFile(file), "application/pdf");
            //StartActivity(intent);
        }

        private void GerarPDF()
        {
            PDFGeneratorHelper helper = new PDFGeneratorHelper();
            string path = helper.GeneratePDF(pedido);

            SendPDFToEmail(pedido);
        }

        private void PermissoesGarantidas()
        {
            if ((ActivityCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Permission.Granted))
            {
                PROSSEGUIR = true;
                GerarPDF();
            }
            else
            {
                string[] permissionRequest = { Manifest.Permission.ReadExternalStorage };
                RequestPermissions(permissionRequest, 0);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if ((requestCode == 0))
            {
                if (grantResults[0] == Permission.Granted)
                {
                    PROSSEGUIR = true;
                    GerarPDF();
                }
                else
                    Toast.MakeText(this, "N�o � poss�vel exportar o arquivo PDF sem as devidas permiss�es", ToastLength.Long).Show();
                return;
            }
        }

        private void SendPDFToEmail(Pedido pedido)
        {
            Intent intent = new Intent(Intent.ActionSend);
            intent.SetType("*/*");

            Java.IO.File file = new Java.IO.File((Android.OS.Environment.ExternalStorageDirectory + "/W Venda - PDFs"), "Pedido " + pedido.id_codigo.ToString() + ", Cliente " + pedido.ds_cliente.ToString() + ".pdf");
            intent.PutExtra(Intent.ExtraSubject, "Cliente " + pedido.ds_cliente.ToString() + ", Emiss�o: " + pedido.dt_emissao.Value.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm"));
            intent.PutExtra(Intent.ExtraStream, Uri.FromFile(file));
            intent.PutExtra(Intent.ExtraText, "Cliente " + pedido.ds_cliente.ToString() + ", Emiss�o: " + pedido.dt_emissao.Value.ToString("dd/MM/yyyy") + " " + DateTime.Now.ToString("HH:mm"));
            StartActivity(intent);
        }

        private void BtnFinalizar_Click(object sender, EventArgs e)
        {
            if (!ValidateViews())
                return;

            if (pedido != null)
            {
                if (pedido.vl_total == 0)
                {
                    Toast.MakeText(this, "Um pedido n�o pode ser finalizado sem conter itens! ", ToastLength.Long).Show();
                    return;
                }
                else
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);

                    alert.SetTitle("Finalizar pedido? Essa a��o n�o pode ser desfeita!");

                    alert.SetNegativeButton("N�o!", (senderAlert, args) =>
                    {

                    });

                    alert.SetPositiveButton("Sim!", (senderAlert, args) =>
                    {
                        try
                        {
                            pedido.fl_status = 1;
                            Save();


                            if (ValidateViews())
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
            else
            {
                if (txtValor_Total.Text == "0")
                {
                    Toast.MakeText(this, "Um pedido n�o pode ser finalizado sem conter itens", ToastLength.Long).Show();
                    return;
                }
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                string mensagem = data.GetStringExtra("mensagem");

                if (mensagem != null)
                {
                    Toast.MakeText(this, mensagem, ToastLength.Short).Show();
                }

                pedido = new Pedido_Manager().Get(pedido.id);
                BindViews();

                if (pedido.vl_total == 0)
                {
                    btnFinalizar.Visibility = ViewStates.Gone;
                    btnItensPedido.Visibility = ViewStates.Gone;
                    spinnerClientes.Enabled = true;
                    btnGerarPDF.Visibility = ViewStates.Gone;
                    btnItensPedido.Visibility = ViewStates.Gone;
                }
                else if (pedido.vl_total != 0)
                {
                    spinnerClientes.Enabled = false;
                    btnFinalizar.Visibility = ViewStates.Visible;
                    btnItensPedido.Visibility = ViewStates.Visible;
                    btnGerarPDF.Visibility = ViewStates.Visible;
                }

                Intent intent = new Intent();
                SetResult(Result.Ok, intent);
            }
        }

        private void Save()
        {
            if (!ValidateViews())
                return;
            try
            {
                BindModel();

                var ped = new Pedido_Manager();
                ped.Save(pedido);

                Intent intent = new Intent();
                intent.PutExtra("mensagem", ped.Mensagem);
                SetResult(Result.Ok, intent);

                //Finish();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        private void Delete()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            alert.SetTitle("Tem certeza que deseja excluir este pedido?");

            alert.SetNegativeButton("N�o!", (senderAlert, args) =>
            {

            });

            alert.SetPositiveButton("Sim!", (senderAlert, args) =>
            {
                try
                {
                    var ped = new Pedido_Manager();
                    ped.Delete(pedido);

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