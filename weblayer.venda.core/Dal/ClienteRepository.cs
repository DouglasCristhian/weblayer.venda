using System;
using System.Collections.Generic;
using System.Linq;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Dal
{
    public class ClienteRepository
    {
        public string Mensage { get; set; }

        public Cliente Get(int id)
        {
            return Database.GetConnection().Table<Cliente>().Where(x => x.id == id).FirstOrDefault();
        }

        public IList<Cliente> GetBytabPreco(int id_tabpreco)
        {
            return Database.GetConnection().Table<Cliente>().Where(x => x.id_tabelapreco == id_tabpreco).ToList();
        }

        public void Save(Cliente entidade)
        {
            try
            {

                if (entidade.id > 0 && Get(entidade.id) != null)
                    Database.GetConnection().Update(entidade);
                else
                    Database.GetConnection().Insert(entidade);
            }
            catch (Exception e)
            {
                Mensage = $"Falha ao Inserir a entidade {entidade.GetType()}. Erro: {e.Message}";
            }
        }

        public void Delete(Cliente entidade)
        {
            Database.GetConnection().Delete(entidade);
        }

        public IList<Cliente> List()
        {
            return Database.GetConnection().Table<Cliente>().ToList();
        }

        public IList<Cliente> ListFiltro(string filtro)
        {
            return Database.GetConnection().Table<Cliente>().Where(x => x.ds_NomeFantasia.StartsWith(filtro)).ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Cliente() { id_Codigo = "1", ds_NomeFantasia = "UNITY SISTEMAS", ds_RazaoSocial = "XPTO SOFTWARE", ds_Cnpj = "11.111.111/1111-11", id_tabelapreco = 1 });
            Save(new Cliente() { id_Codigo = "2", ds_NomeFantasia = "INVISIBLE TUCS", ds_RazaoSocial = "TPA ONIX", ds_Cnpj = "22.222.222/2222-22", id_tabelapreco = 2 });
        }
    }
}