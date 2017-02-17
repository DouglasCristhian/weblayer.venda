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
                erros = erros + "\n O c�digo da tabela de pre�os � inv�lido! Ele deve ter no m�nimo 2 caracteres!";

            if (obj.ds_descricao.Length < 5)
                erros = erros + "\n A descri��o da tabela deve ter no m�nimo 10 caracteres!";

            //TODO: Devidas exce��es

            var Repository = new TabelaPrecoRepository();
            Repository.Save(obj);

            Mensagem = $"Tabela de pre�os {obj.ds_descricao} atualizada com sucesso";
        }

        public void Delete(TabelaPreco obj)
        {
            var Repository = new TabelaPrecoRepository();
            Repository.Delete(obj);

            Mensagem = $"Tabela de pre�os {obj.ds_descricao} exclu�da com sucesso";
        }
    }
}