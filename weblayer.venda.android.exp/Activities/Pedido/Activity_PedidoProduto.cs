using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.android.exp.Adapters;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.exp.Activities
{
    [Activity(Label = "Produtos do Pedido")]
    public class Activity_PedidoProduto : Activity_Base
    {
        private ListView lstViewProdutos;
        private IList<Produto> lstProdutos;
        private EditText edtFiltro;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_PedidoProduto;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FindViews();
            BindViews();
            FillList();
        }

        private void FindViews()
        {
            lstViewProdutos = FindViewById<ListView>(Resource.Id.listViewProdutos2);
            edtFiltro = FindViewById<EditText>(Resource.Id.edtInformarFiltro2);
        }

        private void BindViews()
        {
            lstViewProdutos.ItemClick += LstViewProdutos_ItemClick;
            edtFiltro.TextChanged += EdtFiltro_TextChanged;
        }

        private void EdtFiltro_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            FillList();
        }

        private void FillList()
        {
            lstProdutos = new Produto_Manager().GetProd(edtFiltro.Text.ToString());
            lstViewProdutos.Adapter = new Adapter_Produto_ListView(this, lstProdutos);
        }

        private void LstViewProdutos_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var ListViewProdutoClick = sender as ListView;
            var t = lstProdutos[e.Position];

            Intent intent = new Intent();
            intent.PutExtra("JsonIdProduto", Newtonsoft.Json.JsonConvert.SerializeObject(t));
            SetResult(Result.Ok, intent);
            Finish();
        }
    }
}