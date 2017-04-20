using System;
using System.Collections.Generic;
using System.Linq;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Dal
{
    public class ProdutoRepository
    {
        public string Mensage { get; set; }

        public Produto Get(int id)
        {
            return Database.GetConnection().Table<Produto>().Where(x => x.id == id).FirstOrDefault();
        }

        public void Save(Produto entidade)
        {
            try
            {
                if (entidade.id > 0)// && Get(entidade.id) != null)
                    Database.GetConnection().Update(entidade);
                else
                    Database.GetConnection().Insert(entidade);
            }
            catch (Exception e)
            {
                Mensage = $"Falha ao Inserir a entidade {entidade.GetType()}. Erro: {e.Message}";
            }
        }

        public void Delete(Produto entidade)
        {
            string msgErro = "";

            var ProdutoTabelaPreco = new ProdutoTabelaPrecoRepository().GetByProd(entidade.id);
            var ped_item = new PedidoItemRepository().GetByProd(entidade.id);

            if (ProdutoTabelaPreco == true)
            {
                msgErro = "Tabelas de Preço e ";
            }

            if (ped_item == true)
            {
                msgErro = msgErro + "Pedidos   ";
            }

            if (msgErro.Length > 0)
                throw new Exception($"O produto não pode ser excluído pois existem {msgErro.Left(msgErro.Length - 3)} vinculadas a ele!");


            Database.GetConnection().Delete(entidade);
        }

        public IList<Produto> List()
        {
            return Database.GetConnection().Table<Produto>().ToList();
        }

        public IList<Produto> ListFiltro(string filtro)
        {
            return Database.GetConnection().Table<Produto>().Where(x => x.ds_nome.Contains(filtro)).OrderBy(x => x.id_codigo).ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Produto() { id_codigo = "01", ds_nome = "PRODUTO EXEMPLO", ds_unimedida = "UN", vl_Venda = 10.99, vl_Lista = 10.99 });
            
        }
    }
}