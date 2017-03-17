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
                    IList<PedidoItem> ItensPedido = new PedidoItemRepository().ListPedItem(entidade.id);
                    int vl_volumeTotal = 0;
                    double vl_descontoTotal = 0;

                    var repoitem = new PedidoItemRepository();
                    var itens = repoitem.List(entidade.id);

                    foreach (var item in itens)
                        vl_totalitens += item.nr_quantidade * item.vl_Venda;

                    foreach (var item in ItensPedido)
                    {
                        vl_volumeTotal += item.nr_quantidade;
                        vl_descontoTotal += item.vl_Desconto;
                    }


                    entidade.id_cliente = cliente;
                    entidade.vl_total = vl_totalitens;
                    entidade.vl_volume = vl_volumeTotal;
                    entidade.vl_descontoTotal = vl_descontoTotal;

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

        public IList<Pedido> ListFiltro(string status, int fl_data)
        {
            //Valores default
            DateTime intervalo_inicio = new DateTime(1900, 01, 01);
            DateTime intervalo_fim = new DateTime(2020, 01, 01);

            var string_status = status.TrimEnd(',');

            if (string.IsNullOrWhiteSpace(status))
                string_status = "0,1,2,3,4,5,6,7,8,9";

            if (fl_data == 1)
            {
                intervalo_inicio = DateHelper.GetStartOfDay(DateTime.Today);
                intervalo_fim = DateHelper.GetEndOfDay(DateTime.Today);
            }

            if (fl_data == 2)
            {
                intervalo_inicio = DateHelper.GetStartOfCurrentWeek();
                intervalo_fim = DateHelper.GetEndOfCurrentWeek();
            }

            if (fl_data == 3)
            {
                intervalo_inicio = DateHelper.GetStartOfCurrentMonth();
                intervalo_fim = DateHelper.GetEndOfCurrentMonth();
            }

            var result = Database.GetConnection().Query<Pedido>($@"SELECT * FROM Pedidos Where fl_status in ({string_status}) and 
            dt_emissao>=@intervalo_inicio and dt_emissao<=@intervalo_fim", intervalo_inicio, intervalo_fim);

            return result.ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Pedido() { id_codigo = "1", dt_emissao = DateTime.Parse("2017/02/07"), id_cliente = 1, ds_cliente = "UNITY SISTEMAS", id_vendedor = 1, ds_vendedor = "Maria Lina", vl_total = 0.00, vl_descontoTotal = 0, vl_volume = 7, ds_MsgPedido = "MensagemPedido1", ds_MsgNF = "MensagemNF1", fl_status = 0 });
            Save(new Pedido() { id_codigo = "2", dt_emissao = DateTime.Parse("2017/03/01"), id_cliente = 2, ds_cliente = "INVISIBLE TUCS", id_vendedor = 2, ds_vendedor = "Saory Emanoelle", vl_total = 0.00, vl_descontoTotal = 0, vl_volume = 8, ds_MsgPedido = "MensagemPedido2", ds_MsgNF = "MensagemNF2", fl_status = 1 });
            Save(new Pedido() { id_codigo = "3", dt_emissao = DateTime.Parse("2017/03/05"), id_cliente = 1, ds_cliente = "NAUGHTY DOG", id_vendedor = 3, ds_vendedor = "Douglas Christian", vl_total = 0.00, vl_descontoTotal = 0, vl_volume = 6, ds_MsgPedido = "MensagemPedido3", ds_MsgNF = "MensagemNF3", fl_status = 2 });
            Save(new Pedido() { id_codigo = "4", dt_emissao = DateTime.Parse("2017/03/11"), id_cliente = 2, ds_cliente = "BETA_103", id_vendedor = 4, ds_vendedor = "Natali BR", vl_total = 0.00, vl_descontoTotal = 0, vl_volume = 7, ds_MsgPedido = "MensagemPedido4", ds_MsgNF = "MensagemNF4", fl_status = 3 });
            Save(new Pedido() { id_codigo = "5", dt_emissao = DateTime.Parse("2017/03/15"), id_cliente = 2, ds_cliente = "INVISIBLE TUCS", id_vendedor = 2, ds_vendedor = "Saory Emanoelle", vl_total = 0.00, vl_descontoTotal = 0, vl_volume = 5, ds_MsgPedido = "MensagemPedido5", ds_MsgNF = "MensagemNF5", fl_status = 2 });
            Save(new Pedido() { id_codigo = "6", dt_emissao = DateTime.Parse("2017/03/01"), id_cliente = 1, ds_cliente = "NAUGHTY DOG", id_vendedor = 3, ds_vendedor = "Douglas Christian", vl_total = 0.00, vl_descontoTotal = 0, vl_volume = 6, ds_MsgPedido = "MensagemPedido6", ds_MsgNF = "MensagemNF6", fl_status = 2 });
            Save(new Pedido() { id_codigo = "7", dt_emissao = DateTime.Parse("2017/01/08"), id_cliente = 2, ds_cliente = "BETA_103", id_vendedor = 4, ds_vendedor = "Natali BR", vl_total = 0.00, vl_descontoTotal = 0, vl_volume = 7, ds_MsgPedido = "MensagemPedido7", ds_MsgNF = "MensagemNF7", fl_status = 3 });
            Save(new Pedido() { id_codigo = "8", dt_emissao = DateTime.Parse("2017/02/16"), id_cliente = 2, ds_cliente = "INVISIBLE TUCS", id_vendedor = 2, ds_vendedor = "Saory Emanoelle", vl_total = 0.00, vl_descontoTotal = 0, vl_volume = 5, ds_MsgPedido = "MensagemPedido8", ds_MsgNF = "MensagemNF8", fl_status = 1 });
            Save(new Pedido() { id_codigo = "9", dt_emissao = DateTime.Parse("2017/02/27"), id_cliente = 1, ds_cliente = "NAUGHTY DOG", id_vendedor = 3, ds_vendedor = "Douglas Christian", vl_total = 0.00, vl_descontoTotal = 0, vl_volume = 6, ds_MsgPedido = "MensagemPedido9", ds_MsgNF = "MensagemNF9", fl_status = 1 });
        }
    }
}
