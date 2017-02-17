using System;
using System.Collections.Generic;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Bll
{
    public class Produto_Manager
    {
        public string Mensagem;

        public Produto Get(int id)
        {
            return new ProdutoRepository().Get(id);
        }

        public IList<Produto> GetProduto(string filtro)
        {
            return new ProdutoRepository().List();
        }

        public IList<Produto> GetProd(string filtro)
        {
            return new ProdutoRepository().ListFiltro(filtro);
        }

        public void Save(Produto obj)
        {
            var erros = "";

            //regras....
            //if (obj.id_codigo.Length < 2) 
            //    erros= erros + "\n O c�digo do produto � inv�lido! Ele deve ter no m�nimo 2 caracteres!";

            //if (obj.ds_nome.Length < 10) 
            //    erros = erros + "\n A descri��o do produto deve ter no m�nimo 10 caracteres!";

            if (erros.Length > 0)
                throw new Exception(erros);

            var Repository = new ProdutoRepository();
            Repository.Save(obj);

            Mensagem = $"Produto {obj.ds_nome} atualizado com sucesso";
        }

        public void Delete(Produto obj)
        {
            var Repository = new ProdutoRepository();
            Repository.Delete(obj);

            Mensagem = $"Produto {obj.ds_nome} exclu�do com sucesso";
        }
    }
}