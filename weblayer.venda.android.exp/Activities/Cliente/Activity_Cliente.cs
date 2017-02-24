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
    [Activity(Label = "Clientes")]
    public class Activity_Cliente : Activity_Base
    {
        private ListView lstViewClientes;
        private IList<Cliente> lstClientes;
        private EditText edtFiltro;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_Clientes;
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
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_help);
            menu.RemoveItem(Resource.Id.action_sair);
            menu.RemoveItem(Resource.Id.action_filtrar);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_adicionar:
                    Intent intent = new Intent();
                    intent.SetClass(this, typeof(Activity_EditarCliente));
                    StartActivityForResult(intent, 0);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void FindViews()
        {
            lstViewClientes = FindViewById<ListView>(Resource.Id.lstViewCliente);
            edtFiltro = FindViewById<EditText>(Resource.Id.edtFiltro);
        }

        private void BindViews()
        {
            lstViewClientes.ItemClick += LstViewClientes_ItemClick;
            edtFiltro.TextChanged += EdtFiltro_TextChanged;
        }

        private void EdtFiltro_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            FillList();
        }

        private void FillList()
        {
            lstClientes = new Cliente_Manager().GetClientes(edtFiltro.Text.ToString());
            lstViewClientes.Adapter = new Adapter_Cliente_ListView(this, lstClientes);
        }

        private void LstViewClientes_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var ListViewClienteClick = sender as ListView;
            var t = lstClientes[e.Position];

            Intent intent = new Intent();
            intent.SetClass(this, typeof(Activity_EditarCliente));

            intent.PutExtra("JsonNotaCli", Newtonsoft.Json.JsonConvert.SerializeObject(t));
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