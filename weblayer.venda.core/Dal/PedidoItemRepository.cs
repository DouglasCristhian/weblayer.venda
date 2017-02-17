using System;
using System.Collections.Generic;
using System.Linq;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Dal
{
    public class PedidoItemRepository
    {
        public string Mensage { get; set; }

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

            //Save(new PedidoItem() { id_pedido = 1, id_produto = 01, ds_produto = "LAPIS DE COR AMARELO", nr_quantidade = 5, vl_item = 15.00 });
            //Save(new PedidoItem() { id_pedido = 2, id_produto = 02, ds_produto = "LAPIS DE COR VERMELHO", nr_quantidade = 6, vl_item = 10.00 });
            //Save(new PedidoItem() { id_pedido = 3, id_produto = 03, ds_produto = "LAPIS DE COR AZUL", nr_quantidade = 7, vl_item = 5.00 });
        }
    }
}