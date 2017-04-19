using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.exp.Adapters
{
    public class Adapter_TabelaPreco_ListView : BaseAdapter<TabelaPreco>
    {
        public IList<TabelaPreco> mItems;
        private Context mContext;

        public Adapter_TabelaPreco_ListView(Context context, IList<TabelaPreco> items)
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

        public override TabelaPreco this[int position]
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Adapter_TabelaPreco_ListView, null, false);
            }

            row.FindViewById<TextView>(Resource.Id.txtCodigoTabelaPreco).Text = "Código: " + mItems[position].id_codigo;
            row.FindViewById<TextView>(Resource.Id.txtDescricaoTabelaPreco).Text = "Descrição: " + mItems[position].ds_descricao;
            //row.FindViewById<TextView>(Resource.Id.txtDescontoMaxTabelaPreco).Text = "Desconto Máximo: " + mItems[position].vl_descontomaximo.ToString("##,##0.00");

            return row;
        }
    }

}