using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.android.exp.Adapters;
using weblayer.venda.android.exp.Fragments;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.exp.Activities
{
    [Activity(Label = "Pedidos")]
    public class Activity_Pedido : Activity_Base
    {
        //public static string MyPREFERENCES = "MyPrefs";
        private ListView lstViewPedido;
        private IList<Pedido> lstPedido;
        private TextView txtPedidos;
        private string status;
        private int dataEmissao;
        Pedido ped;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_Pedido;
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {

                case Resource.Id.action_adicionar:
                    Intent intent = new Intent();
                    intent.SetClass(this, typeof(Activity_EditarPedido));
                    StartActivityForResult(intent, 0);
                    break;

                case Resource.Id.action_help:
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    Fragment_Legendas dialog = new Fragment_Legendas();
                    dialog.Show(transaction, "dialog");
                    break;

                case Resource.Id.action_filtrar://TESTE
                    Intent intent2 = new Intent();
                    intent2.SetClass(this, typeof(Activity_FiltrarPedidos));
                    StartActivityForResult(intent2, 0);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FindViews();
            BindViews();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar, menu);
            menu.RemoveItem(Resource.Id.action_deletar);
            menu.RemoveItem(Resource.Id.action_salvar);
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_sair);
            return base.OnCreateOptionsMenu(menu);
        }

        private void FindViews()
        {
            lstViewPedido = FindViewById<ListView>(Resource.Id.listviewPedido);
            txtPedidos = FindViewById<TextView>(Resource.Id.txtPedidos);
        }

        private void BindViews()
        {
            lstViewPedido.ItemClick += LstViewPedidoItem_ItemClick;
            lstViewPedido.ItemLongClick += LstViewPedido_ItemLongClick;

            if (lstPedido != null)
            {
                txtPedidos.Visibility = ViewStates.Gone;
            }

            Filtro_Checkboxes();
        }

        private void Filtro_Checkboxes()
        {
            var prefs = Application.Context.GetSharedPreferences("MyPrefs", FileCreationMode.WorldWriteable);
            var prefEditor = prefs.Edit();

            int data = prefs.GetInt("Id_DataEmissao", 0);
            dataEmissao = data;

            int valor = prefs.GetInt("CheckBox2131427464", -1);
            if (valor == 0)
            {
                status = status + "0,";
            }

            int valor1 = prefs.GetInt("CheckBox2131427466", -1);
            if (valor1 == 0)
            {
                status = status + "1,";
            }

            int valor2 = prefs.GetInt("CheckBox2131427468", -1);
            if (valor2 == 0)
            {
                status = status + "2,";
            }

            int valor3 = prefs.GetInt("CheckBox2131427470", -1);
            if (valor3 == 0)
            {
                status = status + "3,";
            }

            if ((status == "") || (status == null))
            {
                status = "";
            }

            FillList(status, dataEmissao);
        }

        private void Filtro_DataEmissao()
        {

        }

        private void LstViewPedido_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var ListViewPedidoItemClick = sender as ListView;
            var t = lstPedido[e.Position];
            ped = t;

            if (ped.fl_status == 0)
                return;

            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            Fragment_Status dialog = new Fragment_Status();
            dialog.DialogClosed += Dialog_DialogClosed;
            dialog.Show(transaction, "dialog");
        }

        private void Dialog_DialogClosed(object sender, Helpers.DialogEventArgs e)
        {
            string retorno = e.ReturnValue;
            int id = 0;

            if (retorno == "Finalizado")
            {
                id = 1;
            }
            else if (retorno == "Faturado")
            {
                id = 2;
            }
            else if (retorno == "Entregue")
            {
                id = 3;
            }
            else
                return;

            AtualizarStatusPedido(id);
        }

        private void AtualizarStatusPedido(int id)
        {
            PedidoRepository pedRepo = new PedidoRepository();
            pedRepo.Get(ped.id);
            ped.fl_status = id;
            pedRepo.Save(ped);

            FillList(status, dataEmissao);
        }

        private void FillList(string status, int dataemissao)
        {
            lstPedido = new Pedido_Manager().GetPedidos(status, dataemissao);

            if (lstPedido.Count == 0)
            {
                lstViewPedido.Adapter = new Adapter_Pedido_ListView(this, lstPedido);
                txtPedidos.Visibility = ViewStates.Visible;
                txtPedidos.Text = "Não há pedidos que correspondam aos filtros de pesquisa";
            }
            else
            {
                txtPedidos.Visibility = ViewStates.Gone;
                lstViewPedido.Adapter = new Adapter_Pedido_ListView(this, lstPedido);
            }
        }

        private void LstViewPedidoItem_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var ListViewPedidoItemClick = sender as ListView;
            var t = lstPedido[e.Position];

            Intent intent = new Intent();
            intent.SetClass(this, typeof(Activity_EditarPedido));
            intent.PutExtra("JsonPedido", Newtonsoft.Json.JsonConvert.SerializeObject(t));
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                var mensagem = data.GetStringExtra("mensagem");

                if (mensagem != null)
                {
                    Toast.MakeText(this, mensagem, ToastLength.Short).Show();
                }

                status = data.GetStringExtra("Status");
                dataEmissao = data.GetIntExtra("DataEmissao", 0);
                FillList(status, dataEmissao);
            }
        }
    }
}
