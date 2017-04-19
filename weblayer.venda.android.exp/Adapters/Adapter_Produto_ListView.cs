using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.exp.Adapters
{
    public class Adapter_Produto_ListView : BaseAdapter<Produto>
    {
        public IList<Produto> mItems;
        private Context mContext;

        public Adapter_Produto_ListView(Context context, IList<Produto> items)
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

        public override Produto this[int position]
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Adapter_Produtos_ListView, null, false);
            }

            row.FindViewById<TextView>(Resource.Id.txtCodigoProduto).Text = "Código: " + mItems[position].id_codigo.ToString();
            row.FindViewById<TextView>(Resource.Id.txtNomeProduto).Text = "Nome: " + mItems[position].ds_nome.ToString();
            row.FindViewById<TextView>(Resource.Id.txtUniMedidaProduto).Text = "Unidade de Medida: " + mItems[position].ds_unimedida.ToString();
            row.FindViewById<TextView>(Resource.Id.txtValorProduto).Text = "Preço: " + mItems[position].vl_Lista.ToString("##,##0.00");

            return row;
        }

        private void Filtrar(SearchView letra)
        {
            letra.QueryTextChange += Letra_QueryTextChange;
        }

        private void Letra_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {

        }
    }
}