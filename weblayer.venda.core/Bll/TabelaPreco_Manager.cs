using System.Collections.Generic;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Bll
{
    public class TabelaPreco_Manager
    {
        public string Mensagem;

        public TabelaPreco Get(int id)
        {
            return new TabelaPrecoRepository().Get(id);
        }

        public IList<TabelaPreco> GetTabelaPreco(string filtro)
        {
            return new TabelaPrecoRepository().List();
        }

        public void Save(TabelaPreco obj)
        {
            var erros = "";

            //regras....
            if (obj.id_codigo.Length < 2)
                erros = erros + "\n O código da tabela de preços é inválido! Ele deve ter no mínimo 2 caracteres!";

            if (obj.ds_descricao.Length < 5)
                erros = erros + "\n A descrição da tabela deve ter no mínimo 10 caracteres!";

            //TODO: Devidas exceções

            var Repository = new TabelaPrecoRepository();
            Repository.Save(obj);

            Mensagem = $"Tabela de preços {obj.ds_descricao} atualizada com sucesso";
        }

        public void Delete(TabelaPreco obj)
        {
            var Repository = new TabelaPrecoRepository();
            Repository.Delete(obj);

            Mensagem = $"Tabela de preços {obj.ds_descricao} excluída com sucesso";
        }
    }
}