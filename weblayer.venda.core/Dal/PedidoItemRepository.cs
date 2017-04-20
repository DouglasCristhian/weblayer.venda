using System;
using System.Collections.Generic;
using System.Linq;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Dal
{
    public class PedidoItemRepository
    {
        public string Mensage { get; set; }

        public bool GetByProd(int id_produto)
        {
            var item = Database.GetConnection().Table<PedidoItem>().Where(x => x.id_produto == id_produto).FirstOrDefault();
            if (item != null)
                return true;
            else
                return false;
        }

        public IList<PedidoItem> ListPedItem(int id_pedido)
        {
            return Database.GetConnection().Table<PedidoItem>().Where(x => x.id_pedido == id_pedido).ToList();
        }

        public void Save(PedidoItem entidade)
        {
            try
            {
                if (entidade.id > 0)
                    Database.GetConnection().Update(entidade);
                else
                    Database.GetConnection().Insert(entidade);

                var pedidorepo = new PedidoRepository();
                var pedido = pedidorepo.Get(entidade.id_pedido);
                pedidorepo.Save(pedido); //atualizar o total do item...


            }
            catch (Exception e)
            {
                Mensage = $"Falha ao Inserir a entidade {entidade.GetType()}. Erro: {e.Message}";
            }
        }

        public void Delete(PedidoItem entidade)
        {
            Database.GetConnection().Delete(entidade);

            var pedidorepo = new PedidoRepository();
            var pedido = pedidorepo.Get(entidade.id_pedido);
            pedidorepo.Save(pedido); //atualizar o total do item...

        }

        public IList<PedidoItem> List()
        {
            return Database.GetConnection().Table<PedidoItem>().ToList();
        }

        public IList<PedidoItem> List(int id_pedido)
        {
            return Database.GetConnection().Table<PedidoItem>().Where(x => x.id_pedido == id_pedido).ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new PedidoItem() { id_pedido = 1, id_produto = 1, ds_produto = "PRODUTO EXEMPLO", nr_quantidade = 1, vl_Lista = 10.99, vl_Venda = 10.99, vl_Desconto = 0 });

        }
    }
}