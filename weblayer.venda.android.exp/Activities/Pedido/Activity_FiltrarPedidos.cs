using Android.App;
using Android.OS;
using Android.Views;
using weblayer.venda.android.exp.Activities;

namespace weblayer.venda.android.exp
{
    [Activity]
    public class Activity_FiltrarPedidos : Activity_Base
    {
        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_FiltrarPedidos;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar_Filtro, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_aceitar:
                    //SALVAR 
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}