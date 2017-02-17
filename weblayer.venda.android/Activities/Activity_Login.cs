using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.Threading;

namespace weblayer.venda.android.Activities
{
    [Activity(MainLauncher = true, NoHistory = true)]
    public class Activity_Login : Activity
    {
        public static string MyPREFERENCES = "MyPrefs";

        Android.Support.V7.Widget.Toolbar toolbar;
        EditText edtServidor, edtUsuario, edtSenha;
        TextView lblmensagem;
        Button btnEntrar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Activity_Login);

            FindViews();
            RestoreForm();

            btnEntrar.Click += BtnEntrar_Click;
        }

        private void FindViews()
        {
            edtServidor = FindViewById<EditText>(Resource.Id.edtServidor);
            edtUsuario = FindViewById<EditText>(Resource.Id.edtUsuario);
            edtSenha = FindViewById<EditText>(Resource.Id.edtSenha);
            btnEntrar = FindViewById<Button>(Resource.Id.btnEntrar);
            lblmensagem = FindViewById<TextView>(Resource.Id.txtMensagem);

            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "W/Venda";
            toolbar.InflateMenu(Resource.Menu.menu_toolbar);
            toolbar.Menu.RemoveItem(Resource.Id.action_refresh);
            toolbar.Menu.RemoveItem(Resource.Id.action_help);
            toolbar.Menu.RemoveItem(Resource.Id.action_sobre);
            toolbar.Menu.RemoveItem(Resource.Id.action_salvar);
            toolbar.Menu.RemoveItem(Resource.Id.action_deletar);
            toolbar.Menu.RemoveItem(Resource.Id.action_adicionar);

            toolbar.MenuItemClick += Toolbar_MenuItemClick;

        }

        private void Toolbar_MenuItemClick(object sender, Android.Support.V7.Widget.Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.action_sair:
                    Finish();
                    break;
            }
        }

        private bool ValidateViews()
        {
            var validacao = true;
            if (edtServidor.Length() == 0)
            {
                validacao = false;
                edtServidor.Error = "Endereço do servidor inválido!";
            }

            if (edtUsuario.Length() == 0)
            {
                validacao = false;
                edtUsuario.Error = "Usuário inválido!";
            }

            if (edtSenha.Length() == 0)
            {
                validacao = false;
                edtSenha.Error = "Senha inválida!";
            }

            return validacao;

        }

        private void BtnEntrar_Click(object sender, EventArgs e)
        {
            SaveForm();

            if (!ValidateViews())
                return;

            var progressDialog = ProgressDialog.Show(this, "Por favor aguarde...", "Verificando os dados...", true);
            new Thread(new ThreadStart(delegate
            {
                System.Threading.Thread.Sleep(1000);

                //LOAD METHOD TO GET ACCOUNT INFO
                RunOnUiThread(() => ExecutarLogin());

                //HIDE PROGRESS DIALOG
                RunOnUiThread(() => progressDialog.Hide());
            })).Start();
        }

        private void ExecutarLogin()
        {
            //TODO: IMPLEMENTAR VERIFICAÇÃO

            StartActivity(typeof(Activity_Home));
        }

        private void RestoreForm()
        {
            var prefs = Application.Context.GetSharedPreferences(MyPREFERENCES, FileCreationMode.WorldReadable);
            var somePref = prefs.GetString("Login", "");
            edtUsuario.Text = somePref;

            somePref = prefs.GetString("Senha", "");
            edtSenha.Text = somePref;

            somePref = prefs.GetString("Servidor", "");
            edtServidor.Text = somePref;
        }

        private void SaveForm()
        {
            var prefs = Application.Context.GetSharedPreferences(MyPREFERENCES, FileCreationMode.WorldWriteable);
            var prefEditor = prefs.Edit();
            prefEditor.PutString("Login", edtUsuario.Text);
            prefEditor.PutString("Senha", edtSenha.Text);
            prefEditor.PutString("Servidor", edtServidor.Text);
            prefEditor.Commit();
        }

    }
}