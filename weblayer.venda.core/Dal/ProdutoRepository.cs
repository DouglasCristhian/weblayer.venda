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
            return Database.GetConnection().Table<Produto>().Where(x => x.ds_nome.Contains(filtro)).ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Produto() { id_codigo = "1111", ds_nome = "LAPIS DE COR AMARELO", ds_unimedida = "CX", vl_Venda = 1.25, vl_Lista = 6.30 });
            Save(new Produto() { id_codigo = "2222", ds_nome = "LAPIS DE COR VERMELHO", ds_unimedida = "PCT", vl_Venda = 2.5, vl_Lista = 5.25 });
            Save(new Produto() { id_codigo = "3333", ds_nome = "LAPIS DE COR AZUL", ds_unimedida = "CX", vl_Venda = 3.45, vl_Lista = 8 });
        }
    }
}