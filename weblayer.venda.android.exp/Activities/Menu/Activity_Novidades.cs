
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace weblayer.venda.android.exp.Activities
{
    [Activity(Label = "Novidades")]
    public class Activity_Novidades : Activity_Base
    {
        private TextView txtNovidades;

        protected override int LayoutResource
        {
            get
            {
                return Resource.Layout.Activity_Novidades;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FindViews();
            BindData();
        }

        private void FindViews()
        {
            txtNovidades = FindViewById<TextView>(Resource.Id.txtNovidades);
        }

        private void BindData()
        {
            txtNovidades.Text = Novidades();
        }

        private string Novidades()
        {
            string Novidades="";

            Novidades = Novidades + "\n\n1.2 (19/06/2017):"
                                     + "\n\n     [Melhoria] Novo item no menu: Contato";

            Novidades = Novidades + "\n\n1.1 (14/03/2017):"
                                     + "\n\n     [Novo] Implementação do filtro de pedidos por data"
                                     + "\n     [Novo] Visualização do status do pedido";

            Novidades = Novidades  + "\n\n1.0 (24/01/2017):"
                                     + "\n\n     [Novo] Implementação do menu Novidades (Via opção 'Sobre')"
                                     + "\n     [Melhorias] Atualização dos ícones do menu";

            
            return Novidades;

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_toolbar, menu);
            menu.RemoveItem(Resource.Id.action_deletar);
            menu.RemoveItem(Resource.Id.action_adicionar);
            menu.RemoveItem(Resource.Id.action_sobre);
            menu.RemoveItem(Resource.Id.action_salvar);
            menu.RemoveItem(Resource.Id.action_help);
            menu.RemoveItem(Resource.Id.action_sair);
            menu.RemoveItem(Resource.Id.action_filtrar);
            menu.RemoveItem(Resource.Id.action_legenda);
            menu.RemoveItem(Resource.Id.action_contato);

            return base.OnCreateOptionsMenu(menu);
        }

    }
}