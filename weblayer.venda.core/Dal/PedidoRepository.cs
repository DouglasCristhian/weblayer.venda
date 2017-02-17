using System;
using System.Collections.Generic;
using System.Linq;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Dal
{
    public class PedidoRepository
    {
        private double vl_totalitens;

        public string Mensage { get; set; }

        public Pedido Get(int id)
        {
            return Database.GetConnection().Table<Pedido>().Where(x => x.id == id).FirstOrDefault();
        }

        public void Save(Pedido entidade)
        {
            try
            {
                if ((entidade.id > 0) && Get(entidade.id) != null)
                {
                    var repoitem = new PedidoItemRepository();
                    var itens = repoitem.List(entidade.id);
                    foreach (var item in itens)
                        vl_totalitens += item.nr_quantidade * item.vl_Venda;

                    entidade.vl_total = vl_totalitens;
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

        public void Delete(Pedido entidade)
        {
            var repoitem = new PedidoItemRepository();

            try
            {
                //Listando os itens e excluindo....
                var itens = repoitem.List(entidade.id);
                foreach (var item in itens)
                    repoitem.Delete(item);

                Database.GetConnection().Delete(entidade);
            }
            catch (Exception e)
            {
                Mensage = $"Falha ao deletar a entidade {entidade.GetType()}. Erro: {e.Message}";
            }


        }

        public IList<Pedido> List()
        {
            return Database.GetConnection().Table<Pedido>().ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Pedido() { id_codigo = "1", dt_emissao = DateTime.Parse("2016/04/01"), id_cliente = 1, ds_cliente = "UNITY SISTEMAS", id_vendedor = 1, ds_vendedor = "Maria Lina", vl_total = 0.00, ds_MsgPedido = "MensagemPedido1", ds_MsgNF = "MensagemNF1", fl_status = 0 });
            Save(new Pedido() { id_codigo = "2", dt_emissao = DateTime.Parse("2016/06/07"), id_cliente = 2, ds_cliente = "INVISIBLE TUCS", id_vendedor = 2, ds_vendedor = "Saory Emanoelle", vl_total = 0.00, ds_MsgPedido = "MensagemPedido2", ds_MsgNF = "MensagemNF2", fl_status = 1 });
            Save(new Pedido() { id_codigo = "3", dt_emissao = DateTime.Parse("2017/03/01"), id_cliente = 1, ds_cliente = "UNITY SISTEMAS", id_vendedor = 3, ds_vendedor = "Douglas Christian", vl_total = 0.00, ds_MsgPedido = "MensagemPedido3", ds_MsgNF = "MensagemNF3", fl_status = 2 });
            Save(new Pedido() { id_codigo = "4", dt_emissao = DateTime.Parse("2017/01/01"), id_cliente = 2, ds_cliente = "INVISIBLE TUCS", id_vendedor = 2, ds_vendedor = "Saory Emanoelle", vl_total = 0.00, ds_MsgPedido = "MensagemPedido4", ds_MsgNF = "MensagemNF4", fl_status = 3 });
            Save(new Pedido() { id_codigo = "5", dt_emissao = DateTime.Parse("2017/01/01"), id_cliente = 1, ds_cliente = "UNITY SISTEMAS", id_vendedor = 2, ds_vendedor = "Saory Emanoelle", vl_total = 0.00, ds_MsgPedido = "MensagemPedido5", ds_MsgNF = "MensagemNF5", fl_status = 4 });
            Save(new Pedido() { id_codigo = "6", dt_emissao = DateTime.Parse("2017/01/01"), id_cliente = 2, ds_cliente = "INVISIBLE TUCS", id_vendedor = 3, ds_vendedor = "Douglas Christian", vl_total = 0.00, ds_MsgPedido = "MensagemPedido6", ds_MsgNF = "MensagemNF6", fl_status = 5 });
            Save(new Pedido() { id_codigo = "7", dt_emissao = DateTime.Parse("2017/01/01"), id_cliente = 1, ds_cliente = "UNITY SISTEMAS", id_vendedor = 1, ds_vendedor = "Maria Lina", vl_total = 0.00, ds_MsgPedido = "MensagemPedido7", ds_MsgNF = "MensagemNF7", fl_status = 6 });
            Save(new Pedido() { id_codigo = "7", dt_emissao = DateTime.Parse("2017/01/01"), id_cliente = 1, ds_cliente = "UNITY SISTEMAS", id_vendedor = 1, ds_vendedor = "Maria Lina", vl_total = 0.00, ds_MsgPedido = "MensagemPedido8", ds_MsgNF = "MensagemNF7", fl_status = 7 });
            Save(new Pedido() { id_codigo = "7", dt_emissao = DateTime.Parse("2017/01/01"), id_cliente = 1, ds_cliente = "UNITY SISTEMAS", id_vendedor = 1, ds_vendedor = "Maria Lina", vl_total = 0.00, ds_MsgPedido = "MensagemPedido9", ds_MsgNF = "MensagemNF7", fl_status = 8 });
            Save(new Pedido() { id_codigo = "7", dt_emissao = DateTime.Parse("2017/01/01"), id_cliente = 1, ds_cliente = "UNITY SISTEMAS", id_vendedor = 1, ds_vendedor = "Maria Lina", vl_total = 0.00, ds_MsgPedido = "MensagemPedido10", ds_MsgNF = "MensagemNF7", fl_status = 9 });
        }
    }
}
