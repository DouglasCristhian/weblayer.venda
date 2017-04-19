using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.exp.Adapters
{
    public class Adapter_ProdTabPreco_ListView : BaseAdapter<ProdutoTabelaPreco>
    {
        public Produto produto;
        public IList<ProdutoTabelaPreco> mitems;
        public Context mContext;

        public Adapter_ProdTabPreco_ListView(Context context, IList<ProdutoTabelaPreco> items)
        {
            mitems = items;
            mContext = context;
        }

        public override int Count
        {
            get
            {
                return mitems.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ProdutoTabelaPreco this[int position]
        {
            get
            {
                return mitems[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Adapter_ProdTabelaPreco_ListView, null, false);
            }

            TabelaPreco tblPreco;
            TabelaPreco_Manager tbl = new TabelaPreco_Manager();
            tblPreco = tbl.Get(mitems[position].id_tabpreco);

            Produto prod;
            Produto_Manager prod_manager = new Produto_Manager();
            prod = prod_manager.Get(mitems[position].id_produto);

            row.FindViewById<TextView>(Resource.Id.txtDescProdutoTblPreco).Text = "Produto: " + prod.ds_nome.ToString();
            row.FindViewById<TextView>(Resource.Id.txtDescTabelaPrecoTblPrecos).Text = "Tabela: " + tblPreco.ds_descricao.ToString();
            row.FindViewById<TextView>(Resource.Id.txtValorPrecos).Text = "Valor: " + mitems[position].vl_Valor.ToString("##,##0.00");

            return row;
        }
    }
}