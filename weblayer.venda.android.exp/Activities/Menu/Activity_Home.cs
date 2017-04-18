using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;

namespace weblayer.venda.android.exp.Activities
{
    [Activity(NoHistory = true, MainLauncher = false)]
    public class Activity_Home : Activity
    {
        Android.Support.V7.Widget.Toolbar toolbar;
        private List<string> ItensLista;
        private ListView ListViewHome;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Activity_Home);

            core.Dal.Database.Initialize();

            FindViews();
            BindData();
        }

        private void Toolbar_MenuItemClick(object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.action_sobre:
                    StartActivity(typeof(Activity_Sobre));
                    break;

                case Resource.Id.action_help:
                    StartActivity(typeof(Activity_WebView));
                    //Intent intent = new Intent(this, typeof(Activity_WebView));
                    break;

                case Resource.Id.action_sair:
                    Finish();
                    break;
            }
        }

        private void FindViews()
        {
            ListViewHome = FindViewById<ListView>(Resource.Id.listviewHome);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "W/Vendas Express";
            toolbar.InflateMenu(Resource.Menu.menu_toolbar);

            toolbar.Menu.RemoveItem(Resource.Id.action_deletar);
            toolbar.Menu.RemoveItem(Resource.Id.action_adicionar);
            toolbar.Menu.RemoveItem(Resource.Id.action_salvar);
            toolbar.Menu.RemoveItem(Resource.Id.action_filtrar);
            toolbar.Menu.RemoveItem(Resource.Id.action_legenda);
        }

        private void BindData()
        {
            ListViewHome.ItemClick += ListViewHome_ItemClick;

            ItensLista = new List<string>();
            ItensLista.Add("Produtos");
            ItensLista.Add("Clientes");
            ItensLista.Add("Tabela de Preços");
            ItensLista.Add("Preços por Produto");
            ItensLista.Add("Pedidos");

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ItensLista);
            ListViewHome.Adapter = adapter;

            toolbar.MenuItemClick += Toolbar_MenuItemClick;
        }

        private void ListViewHome_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

            switch (e.Position)
            {
                case 0:
                    StartActivity(typeof(Activity_Produto));
                    break;

                case 1:
                    StartActivity(typeof(Activity_Cliente));
                    break;

                case 2:
                    StartActivity(typeof(Activity_TabelaPreco));
                    break;

                case 3:
                    StartActivity(typeof(Activity_ProdTabPreco));
                    break;

                case 4:
                    StartActivity(typeof(Activity_Pedido));
                    break;

                default:
                    break;
            }
        }
    }
}