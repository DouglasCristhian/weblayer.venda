using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.Adapters
{
    public class Adapter_PedidoItem_ListView : BaseAdapter<PedidoItem>
    {
        public IList<PedidoItem> mItems;
        private Context mContext;

        public Adapter_PedidoItem_ListView(Context context, IList<PedidoItem> items)
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

        public override PedidoItem this[int position]
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Adapter_PedidoItem_ListView, null, false);
            }

            row.FindViewById<TextView>(Resource.Id.txtIdProdutoPedidoItem).Text = "Descrição do Produto: " + mItems[position].ds_produto.ToString();
            row.FindViewById<TextView>(Resource.Id.txtValorPedidoItem).Text = "Valor da Lista: " + mItems[position].vl_Lista.ToString("##,##0.00");
            row.FindViewById<TextView>(Resource.Id.txtValorPedidoItem).Text = "Valor de Venda: " + mItems[position].vl_Venda.ToString("##,##0.00");
            row.FindViewById<TextView>(Resource.Id.txtValorPedidoItem).Text = "Desconto: " + mItems[position].vl_Desconto.ToString("##,##0.00");
            row.FindViewById<TextView>(Resource.Id.txtQuantidadePedidoItem).Text = "Quantidade: " + mItems[position].nr_quantidade.ToString();

            double go = double.Parse(mItems[position].nr_quantidade.ToString()) * double.Parse(mItems[position].vl_Venda.ToString());
            row.FindViewById<TextView>(Resource.Id.txtValorTotalPedidoItem).Text = "Valor Total: " + go.ToString("##,##0.00");

            return row;
        }
    }
}