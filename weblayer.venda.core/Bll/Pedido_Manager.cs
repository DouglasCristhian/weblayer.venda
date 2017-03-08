using System.Collections.Generic;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Bll
{
    public class Pedido_Manager
    {
        public string Mensagem;

        public Pedido Get(int id)
        {
            return new PedidoRepository().Get(id);
        }

        public IList<Pedido> GetPedidos(string filtro, int dataemissao)
        {
            return new PedidoRepository().ListFiltro(filtro, dataemissao);
        }

        public IList<Pedido> GetPedidoUnico(string filtro)
        {
            return new PedidoRepository().List();
        }

        public void Save(Pedido obj)
        {
            /* var erros = "";

             //regras....
             if (obj.id_Codigo.Length < 2)
                 erros = erros + "\n O c�digo do pedido deve ter no m�nimo 2 caracteres!";

             if (obj.id_cliente.Length < 5)
                 erros = erros + "\n A descri��o do produto deve ter no m�nimo 10 caracteres!";

             //TODO: Devidas exce��es

             if (erros.Length > 0)
                 throw new Exception(erros);*/

            var Repository = new PedidoRepository();
            Repository.Save(obj);

            Mensagem = $"Pedido {obj.id_codigo} atualizado com sucesso";
        }

        public void Delete(Pedido obj)
        {
            var Repository = new PedidoRepository();
            Repository.Delete(obj);

            Mensagem = $"Pedido {obj.id_codigo} exclu�do com sucesso";
        }


    }
}