using System;
using System.Collections.Generic;
using System.Linq;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Dal
{
    public class TabelaPrecoRepository
    {
        public string Mensage { get; set; }

        public TabelaPreco Get(int id)
        {
            return Database.GetConnection().Table<TabelaPreco>().Where(x => x.id == id).FirstOrDefault();
        }

        public IList<TabelaPreco> GetByProd(int id_produto)
        {
            return Database.GetConnection().Table<TabelaPreco>().Where(x => x.id == id_produto).ToList();
        }

        public void Save(TabelaPreco entidade)
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

        public void Delete(TabelaPreco entidade)
        {
            var clientes = new ClienteRepository().GetBytabPreco(entidade.id);
            if (clientes.Count > 0)
            {
                throw new Exception("Tabela de preço não pode ser excluída pois existem clientes vinculados a ela!");
            }

            Database.GetConnection().Delete(entidade);
        }

        public IList<TabelaPreco> List()
        {
            return Database.GetConnection().Table<TabelaPreco>().ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new TabelaPreco() { id_codigo = "11", ds_descricao = "NORMAL", vl_valor = 5.00, vl_descontomaximo = 5 });
            Save(new TabelaPreco() { id_codigo = "22", ds_descricao = "PROMOÇÃO", vl_valor = 12.00, vl_descontomaximo = 3 });
        }

    }
}