
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.android.exp.Adapters;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.exp.Activities
{
    [Activity(Label = "Produtos")]
    public class Activity_Produto : Activity_Base
    {
        private ListView lstViewProdutos;
        private IList<Produto> lstProdutos;
        private EditText edtFiltro;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_Produtos;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar, menu);
            menu.RemoveItem(Resource.Id.action_salvar);
            menu.RemoveItem(Resource.Id.action_deletar);
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_help);
            menu.RemoveItem(Resource.Id.action_sair);
            menu.RemoveItem(Resource.Id.action_filtrar);
            menu.RemoveItem(Resource.Id.action_legenda);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_adicionar:
                    Intent intent = new Intent();
                    intent.SetClass(this, typeof(Activity_EditarProduto));
                    StartActivityForResult(intent, 0);
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

        private void FindViews()
        {
            lstViewProdutos = FindViewById<ListView>(Resource.Id.listViewProdutos);
            edtFiltro = FindViewById<EditText>(Resource.Id.edtFiltro);
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
            intent.SetClass(this, typeof(Activity_EditarProduto));

            intent.PutExtra("JsonNotaProd", Newtonsoft.Json.JsonConvert.SerializeObject(t));
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                var mensagem = data.GetStringExtra("mensagem");
                Toast.MakeText(this, mensagem, ToastLength.Short).Show();

                FillList();
            }
        }
    }
}