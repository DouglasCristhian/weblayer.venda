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

        public bool GetByCliente(int id_cliente)
        {
            var item = Database.GetConnection().Table<Pedido>().Where(x => x.id_cliente == id_cliente).FirstOrDefault();
            if (item != null)
                return true;
            else
                return false;
        }

        public IList<Pedido> GetPed(Cliente cli)
        {
            return Database.GetConnection().Table<Pedido>().Where(x => x.id_cliente == cli.id).ToList();
        }

        public void Save(Pedido entidade)
        {
            try
            {
                if ((entidade.id > 0) && Get(entidade.id) != null)
                {
                    var repoCli = new ClienteRepository();
                    var cliente = repoCli.Get(entidade.id_cliente).id;

                    var repoitem = new PedidoItemRepository();
                    var itens = repoitem.List(entidade.id);
                    foreach (var item in itens)
                        vl_totalitens += item.nr_quantidade * item.vl_Venda;

                    entidade.id_cliente = cliente;
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

        public IList<Pedido> List(string status, int fl_data)
        {
            string string_status;

            //se data = 0, todos
            //se data = 1, pedidosdodia
            //se data = 2, pedidosdasemana
            //se data = 3, pedidosdomes

            if (fl_data == 0)
            {
                string_status = "0";
            }

            if (fl_data == 1)
            {
                string_status = "1";
            }


            if (fl_data == 2)
            {
                string_status = "2";
            }


            if (fl_data == 3)
            {
                string_status = "3";
            }

            if ((status == "") || (status == null))
            {
                string_status = "";
            }
            else
                string_status = status.TrimEnd(',');

            if ((status != "") && (status != null))
            {
                var arr_status = Array.ConvertAll(string_status.Split(','), int.Parse).ToList();

                var result = from x in Database.GetConnection().Table<Pedido>().ToList()
                             where (arr_status.Contains(x.fl_status))
                             //&& (dataemissao.ToString().Contains(x.dt_emissao.ToString()))
                             select x;

                return result.ToList();

                //return Database.GetConnection().Table<Pedido>().Where(x => arr_status.Contains(x.fl_status)).ToList();
            }
            else
            {
                return Database.GetConnection().Table<Pedido>().ToList();
            }
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Pedido() { id_codigo = "1", dt_emissao = DateTime.Parse("2016/04/01"), id_cliente = 1, ds_cliente = "UNITY SISTEMAS", id_vendedor = 1, ds_vendedor = "Maria Lina", vl_total = 0.00, ds_MsgPedido = "MensagemPedido1", ds_MsgNF = "MensagemNF1", fl_status = 0 });
            Save(new Pedido() { id_codigo = "2", dt_emissao = DateTime.Parse("2016/06/07"), id_cliente = 2, ds_cliente = "INVISIBLE TUCS", id_vendedor = 2, ds_vendedor = "Saory Emanoelle", vl_total = 0.00, ds_MsgPedido = "MensagemPedido2", ds_MsgNF = "MensagemNF2", fl_status = 1 });
            Save(new Pedido() { id_codigo = "3", dt_emissao = DateTime.Parse("2017/03/01"), id_cliente = 1, ds_cliente = "NAUGHTY DOG", id_vendedor = 3, ds_vendedor = "Douglas Christian", vl_total = 0.00, ds_MsgPedido = "MensagemPedido3", ds_MsgNF = "MensagemNF3", fl_status = 2 });
            Save(new Pedido() { id_codigo = "4", dt_emissao = DateTime.Parse("2017/01/01"), id_cliente = 2, ds_cliente = "BETA_103", id_vendedor = 4, ds_vendedor = "Natali BR", vl_total = 0.00, ds_MsgPedido = "MensagemPedido4", ds_MsgNF = "MensagemNF4", fl_status = 3 });
        }
    }
}
