
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.android.Adapters;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.Activities
{
    [Activity(Label = "Tabelas de Preço")]
    public class Activity_TabelaPreco : Activity_Base
    {
        private ListView lstViewTabelaPrecos;
        private IList<TabelaPreco> lstTabelaPrecos;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_TabelaPrecos;
            }
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
            menu.RemoveItem(Resource.Id.action_salvar);
            menu.RemoveItem(Resource.Id.action_deletar);
            menu.RemoveItem(Resource.Id.action_adicionar);
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_refresh);
            menu.RemoveItem(Resource.Id.action_help);
            menu.RemoveItem(Resource.Id.action_sair);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_adicionar:
                    Intent intent = new Intent();
                    intent.SetClass(this, typeof(Activity_EditarTabelaPreco));
                    StartActivityForResult(intent, 0);
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FindViews()
        {
            lstViewTabelaPrecos = FindViewById<ListView>(Resource.Id.listViewTabelaPrecos);
        }

        private void BindViews()
        {
            lstViewTabelaPrecos.ItemClick += LstViewTabelaPrecos_Click;
        }

        private void FillList()
        {
            lstTabelaPrecos = new TabelaPreco_Manager().GetTabelaPreco("");
            lstViewTabelaPrecos.Adapter = new Adapter_TabelaPreco_ListView(this, lstTabelaPrecos);
        }

        private void LstViewTabelaPrecos_Click(object sender, AdapterView.ItemClickEventArgs e)
        {
            var ListViewTabelaPreco = sender as ListView;
            var t = lstTabelaPrecos[e.Position];

            Intent intent = new Intent();
            intent.SetClass(this, typeof(Activity_EditarTabelaPreco));

            intent.PutExtra("JsonNotaTabela", Newtonsoft.Json.JsonConvert.SerializeObject(t));
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