using System.Collections.Generic;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Bll
{
    public class ProdutoTabelaPreco_Manager
    {
        public string Mensagem;

        public ProdutoTabelaPreco Get(int id_tabelapreco, int id_produto)
        {
            return new ProdutoTabelaPrecoRepository().Get(id_tabelapreco, id_produto);
        }

        public IList<ProdutoTabelaPreco> GetProdTabPreco(string filtro)
        {
            return new ProdutoTabelaPrecoRepository().List();
        }

        public void Save(ProdutoTabelaPreco obj)
        {
            var Repository = new ProdutoTabelaPrecoRepository();
            Repository.Save(obj);

            Mensagem = $"Tabela de preços {obj.id} foi atualizada com sucesso!";
        }

        public void Delete(ProdutoTabelaPreco obj)
        {
            var Repository = new ProdutoTabelaPrecoRepository();
            Repository.Delete(obj);

            Mensagem = $"Tabela de preços {obj.id} foi excluída com sucesso!";
        }
    }
}