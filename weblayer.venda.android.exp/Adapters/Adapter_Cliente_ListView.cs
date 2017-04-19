using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.exp.Adapters
{
    [Activity(Label = "Adapter_Cliente_ListView")]
    public class Adapter_Cliente_ListView : BaseAdapter<Cliente>
    {
        public IList<Cliente> mItems;
        private Context mContext;

        public Adapter_Cliente_ListView(Context context, IList<Cliente> items)
        {
            mItems = items;
            mContext = context;
        }

        public override int Count
        {
            get
            {
                return mItems.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Cliente this[int position]
        {
            get
            {
                return mItems[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Adapter_Clientes_ListView, null, false);
            }

            row.FindViewById<TextView>(Resource.Id.txtCodigoCliente).Text = "Código: " + mItems[position].id_Codigo.ToString();
            row.FindViewById<TextView>(Resource.Id.txtRazaoSocial).Text = "Razão Social: " + mItems[position].ds_RazaoSocial.ToString();
            row.FindViewById<TextView>(Resource.Id.txtNomeFantasia).Text = "Nome Fantasia: " + mItems[position].ds_NomeFantasia.ToString();
            row.FindViewById<TextView>(Resource.Id.txtCNPJ).Text = "CNPJ: " + mItems[position].ds_Cnpj.ToString();

            TabelaPreco tblPreco;
            TabelaPreco_Manager tbl = new TabelaPreco_Manager();
            tblPreco = tbl.Get(mItems[position].id_tabelapreco);

            row.FindViewById<TextView>(Resource.Id.txtTabelaPrecoCli).Text = "Tabela de Preço: " + tblPreco.ds_descricao.ToString();

            return row;
        }
    }
}