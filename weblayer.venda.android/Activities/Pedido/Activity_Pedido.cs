using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.android.Adapters;
using weblayer.venda.android.Fragments;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.Activities
{
    [Activity(Label = "Pedidos")]
    public class Activity_Pedido : Activity_Base
    {
        private ListView lstViewPedido;
        private IList<Pedido> lstPedido;
        private int dataEmissao;
        private string status = "";

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

                case Resource.Id.action_filtrar:
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
            Filtro_Checkboxes();
            //FillList(status, dataEmissao);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar, menu);
            menu.RemoveItem(Resource.Id.action_deletar);
            menu.RemoveItem(Resource.Id.action_salvar);
            menu.RemoveItem(Resource.Id.action_refresh);
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_sair);
            return base.OnCreateOptionsMenu(menu);
        }

        private void FindViews()
        {
            lstViewPedido = FindViewById<ListView>(Resource.Id.listviewPedido);
        }

        private void Filtro_Checkboxes()
        {
            var prefs = Application.Context.GetSharedPreferences("MyPrefs", FileCreationMode.WorldWriteable);
            var prefEditor = prefs.Edit();

            int data = prefs.GetInt("Id_DataEmissao", 0);
            dataEmissao = data;

            int valor = prefs.GetInt("CheckBox0", -1);
            if (valor == 0)
            {
                status = status + "0,";
            }

            int valor1 = prefs.GetInt("CheckBox1", -1);
            if (valor1 == 0)
            {
                status = status + "1,";
            }

            int valor2 = prefs.GetInt("CheckBox2", -1);
            if (valor2 == 0)
            {
                status = status + "2,";
            }

            int valor3 = prefs.GetInt("CheckBox3", -1);
            if (valor3 == 0)
            {
                status = status + "3,";
            }

            int valor4 = prefs.GetInt("CheckBox4", -1);
            if (valor4 == 0)
            {
                status = status + "4,";
            }

            int valor5 = prefs.GetInt("CheckBox5", -1);
            if (valor5 == 0)
            {
                status = status + "5,";
            }

            int valor6 = prefs.GetInt("CheckBox6", -1);
            if (valor6 == 0)
            {
                status = status + "6,";
            }

            int valor7 = prefs.GetInt("CheckBox7", -1);
            if (valor7 == 0)
            {
                status = status + "7,";
            }

            int valor8 = prefs.GetInt("CheckBox8", -1);
            if (valor8 == 0)
            {
                status = status + "8,";
            }

            int valor9 = prefs.GetInt("CheckBox9", -1);
            if (valor9 == 0)
            {
                status = status + "9,";
            }
            if ((status == "") || (status == null))
            {
                status = "";
            }

            FillList(status, dataEmissao);
        }

        private void BindViews()
        {
            lstViewPedido.ItemClick += LstViewPedidoItem_ItemClick;
        }

        private void FillList(string filtro, int dataemissao)
        {
            lstPedido = new Pedido_Manager().GetPedidos(filtro, dataemissao);
            lstViewPedido.Adapter = new Adapter_Pedido_ListView(this, lstPedido);
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
                if (status == null)
                {
                    status = "";
                }
                dataEmissao = data.GetIntExtra("DataEmissao", 0);
                FillList(status, dataEmissao);
            }
        }
    }
}
