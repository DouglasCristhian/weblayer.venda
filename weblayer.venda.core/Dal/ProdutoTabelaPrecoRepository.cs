using System;
using System.Collections.Generic;
using System.Linq;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Dal
{
    public class ProdutoTabelaPrecoRepository
    {
        public string Mensagem;

        public ProdutoTabelaPreco Get(int id)
        {
            return Database.GetConnection().Table<ProdutoTabelaPreco>().Where(x => x.id == id).FirstOrDefault();
        }

        public ProdutoTabelaPreco Get(int id_tabelapreco, int id_produto)
        {
            return Database.GetConnection().Table<ProdutoTabelaPreco>().Where(x => x.id_tabpreco == id_tabelapreco && x.id_produto == id_produto).FirstOrDefault();
        }

        public void Save(ProdutoTabelaPreco entidade)
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
                Mensagem = $"Falha ao Inserir a entidade {entidade.GetType()}. Erro: {e.Message}";
            }
        }

        public void Delete(ProdutoTabelaPreco entidade)
        {
            Database.GetConnection().Delete(entidade);
        }

        public IList<ProdutoTabelaPreco> List()
        {
            return Database.GetConnection().Table<ProdutoTabelaPreco>().ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new ProdutoTabelaPreco() { id_produto = 1, id_tabpreco = 1, vl_Valor = 15.00 });
            Save(new ProdutoTabelaPreco() { id_produto = 2, id_tabpreco = 1, vl_Valor = 20.00 });
            Save(new ProdutoTabelaPreco() { id_produto = 3, id_tabpreco = 2, vl_Valor = 25.00 });
        }

    }
}