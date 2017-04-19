using System;
using System.Collections.Generic;
using System.Linq;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Dal
{
    public class PedidoRepository
    {
        //private double vl_totalitens;
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
                    int vl_volumeTotal = 0;
                    double vl_descontoTotal = 0;
                    double vl_totalitens = 0;

                    var repoitem = new PedidoItemRepository();
                    var itens = repoitem.List(entidade.id);

                    foreach (var item in itens)
                    {
                        vl_totalitens += item.nr_quantidade * item.vl_Venda;
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

            return result.OrderBy(x => -x.id).ToList();
        }

        public void MakeDataMock()
        {
            if (List().Count > 0)
                return;

            Save(new Pedido() { id_codigo = "1", dt_emissao = DateTime.Parse("2017/02/07"), id_cliente = 1, ds_cliente = "CLIENTE EXEMPLO 1", id_vendedor = 1, ds_vendedor = "", vl_total = 10.99, vl_descontoTotal = 0, vl_volume = 1, ds_MsgPedido = "MensagemPedido1", ds_MsgNF = "MensagemNF1", fl_status = 0 });
            
        }
    }
}
