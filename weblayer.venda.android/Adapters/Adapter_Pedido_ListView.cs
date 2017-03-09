using Android.Content;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using weblayer.venda.core.Model;

namespace weblayer.venda.android.Adapters
{
    public class Adapter_Pedido_ListView : BaseAdapter<Pedido>
    {
        public IList<Pedido> mItems;
        private Context mContext;

        public Adapter_Pedido_ListView(Context context, IList<Pedido> items)
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

        public override Pedido this[int position]
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.Adapter_Pedido_ListView, null, false);
            }

            //row.FindViewById<TextView>(Resource.Id.txtId_Pedido).Text = "Código Pedido: " + mItems[position].id_codigo.ToString();
            row.FindViewById<TextView>(Resource.Id.txtId_Cliente).Text = "Cliente: " + mItems[position].ds_cliente.ToString();
            row.FindViewById<TextView>(Resource.Id.txtId_Vendedor).Text = "Vendedor: " + mItems[position].ds_vendedor.ToString();
            row.FindViewById<TextView>(Resource.Id.txtValor_Total).Text = "Valor Total: " + mItems[position].vl_total.ToString("##,##0.00");
            row.FindViewById<TextView>(Resource.Id.txtData_Emissao).Text = "Data de Emissão " + mItems[position].dt_emissao.Value.ToString("dd/MM/yyyy");
            //row.FindViewById<TextView>(Resource.Id.txt_Observacao).Text = "Observação: " + mItems[position].ds_observacao.ToString();

            if (mItems[position].fl_status == 0)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.BarrinhaCinzaClaro);
            }

            if (mItems[position].fl_status == 1)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.BarrinhaCinzaEscuro);
            }


            if (mItems[position].fl_status == 2)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.BarrinhaAzul);
            }


            if (mItems[position].fl_status == 3)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.BarrinhaRoxa);
            }

            if (mItems[position].fl_status == 4)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.BarrinhaVermelhaParcial);
            }

            if (mItems[position].fl_status == 5)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.BarrinhaVermelha);
            }

            if (mItems[position].fl_status == 6)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.BarrinhaAmarelaParcial);
            }


            if (mItems[position].fl_status == 7)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.BarrinhaAmarela);
            }


            if (mItems[position].fl_status == 8)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.BarrinhaVerdeParcial);
            }


            if (mItems[position].fl_status == 9)
            {
                row.FindViewById<ImageView>(Resource.Id.imgView).SetBackgroundResource(Resource.Drawable.BarrinhaVerde);
            }


            return row;
        }
    }
}