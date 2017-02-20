using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.android.exp.Adapters;
using weblayer.venda.android.exp.Fragments;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.exp.Activities
{
    [Activity(Label = "Pedidos")]
    public class Activity_Pedido : Activity_Base
    {
        private ListView lstViewPedido;
        private IList<Pedido> lstPedido;

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
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FindViews();
            BindViews();
            FillList();
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
        }

        private void BindViews()
        {
            lstViewPedido.ItemClick += LstViewPedidoItem_ItemClick;
        }

        private void FillList()
        {
            lstPedido = new Pedido_Manager().GetPedidos("");
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

                FillList();
            }
        }
    }
}
