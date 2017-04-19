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

        public bool GetByTabPreco(int id_tabpreco)
        {
            var item = Database.GetConnection().Table<Cliente>().Where(x => x.id_tabelapreco == id_tabpreco).FirstOrDefault();
            if (item != null)
                return true;
            else
                return false;
        }

        public void Save(Cliente entidade)
        {
            try
            {
                if (entidade.id > 0 && Get(entidade.id) != null)
                {
                    PedidoRepository ped = new PedidoRepository();
                    var pedido = ped.GetPed(entidade);
                    foreach (var item in pedido)
                    {
                        item.ds_cliente = entidade.ds_NomeFantasia;
                        ped.Save(item);
                    }


                    Database.GetConnection().Update(entidade);
                }
                else
                {
                    Database.GetConnection().Insert(entidade);
                }
            }
            catch (Exception e)
            {
                Mensage = $"Falha ao Inserir a entidade {entidade.GetType()}. Erro: {e.Message}";
            }
        }

        public void Delete(Cliente entidade)
        {
            string msgerro = "";

            var clientes = new PedidoRepository().GetByCliente(entidade.id);

            if (clientes == true)
            {
                msgerro = "Pedidos e ";
            }

            if (msgerro.Length > 0)
                throw new Exception($"Cliente não pode ser excluído pois existem {msgerro.Left(msgerro.Length - 3)} vinculados a ela!");

            Database.GetConnection().Delete(entidade);
        }

        public IList<Cliente> List()
        {
            return Database.GetConnection().Table<Cliente>().ToList();
        }

        public IList<Cliente> ListFiltro(string filtro)
        {
            return Database.GetConnection().Table<Cliente>().Where(x => x.ds_NomeFantasia.StartsWith(filtro) || x.ds_RazaoSocial.StartsWith(filtro)).OrderBy(x => x.id).ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Cliente() { id_Codigo = "1", ds_NomeFantasia = "CLIENTE EXEMPLO 1", ds_RazaoSocial = "CLIENTE EXEMPLO 1 LTDA", ds_Cnpj = "11.111.111/1111-11", id_tabelapreco = 1 });
            
        }
    }
}