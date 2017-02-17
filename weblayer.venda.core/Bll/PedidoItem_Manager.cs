using System.Collections.Generic;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Bll
{
    public class PedidoItem_Manager
    {
        public string Mensagem;

        public IList<PedidoItem> GetPedidoItem(int id_pedido)
        {
            return new PedidoItemRepository().List(id_pedido);
        }

        public void Save(PedidoItem obj)
        {
            var Repository = new PedidoItemRepository();
            Repository.Save(obj);

            Mensagem = $"Item do pedido {obj.id_pedido} atualizado com sucesso";

        }

        public void Delete(PedidoItem obj)
        {
            var Repository = new PedidoItemRepository();
            Repository.Delete(obj);

            Mensagem = $"Item do pedido {obj.id_pedido} excluído com sucesso";
        }
    }
}