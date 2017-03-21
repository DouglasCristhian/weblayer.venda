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

            string msgerro = "";

            var clientes = new ClienteRepository().GetByTabPreco(entidade.id);
            var ProdutoTabelaPreco = new ProdutoTabelaPrecoRepository().GetByTab(entidade.id);

            if (clientes == true)
            {
                msgerro = "Clientes e ";
            }

            if (ProdutoTabelaPreco == true)
            {
                msgerro = msgerro + "Produtos   ";
            }

            if (msgerro.Length > 0)
                throw new Exception($"Tabela de preço não pode ser excluída pois existem {msgerro.Left(msgerro.Length - 3)} vinculados a ela!");

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

            Save(new TabelaPreco() { id_codigo = "1", ds_descricao = "TABELA_NORMAL"/* vl_valor = 5.00, vl_descontomaximo = 5*/ });
            Save(new TabelaPreco() { id_codigo = "2", ds_descricao = "TABELA_PROMOÇÃO"/*, vl_valor = 12.00, vl_descontomaximo = 3*/ });
        }

    }
}