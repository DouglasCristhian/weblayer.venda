using System.Collections.Generic;
using weblayer.venda.core.Sinc.Model;

namespace weblayer.venda.android.Sinc
{
    public class WebServiceMock
    {
        public IList<Cliente> GetClientes()
        {

            var clientes = new List<Cliente>();

            clientes.Add(new Cliente { id = 1, ds_Cnpj = "99999999999999", ds_NomeFantasia = "Cliente vindo do webservice 1", ds_RazaoSocial = "Teste 1", id_Codigo = "00001", id_tabelapreco = 1 });
            clientes.Add(new Cliente { id = 2, ds_Cnpj = "88888888888888", ds_NomeFantasia = "Cliente vindo do webservice 2", ds_RazaoSocial = "Teste 2", id_Codigo = "00002", id_tabelapreco = 2 });
            clientes.Add(new Cliente { id = 3, ds_Cnpj = "77777777777777", ds_NomeFantasia = "Cliente vindo do webservice 3", ds_RazaoSocial = "Teste 3", id_Codigo = "00003", id_tabelapreco = 1 });

            return clientes;
        }

        public IList<Produto> GetProdutos()
        {

            var produtos = new List<Produto>();

            produtos.Add(new Produto { id = 1, ds_nome = "LAPIS VERMELHO", ds_unimedida = "CX", id_codigo = "001",/* id_tabpreco = 1,*/ vl_Lista = 5.25 });
            produtos.Add(new Produto { id = 2, ds_nome = "BOLO DE LARANJA", ds_unimedida = "PCT", id_codigo = "002", /*id_tabpreco = 1,*/ vl_Lista = 11.75 });
            produtos.Add(new Produto { id = 3, ds_nome = "BALAS SORTIDA", ds_unimedida = "PCT", id_codigo = "003", /*id_tabpreco = 2,*/ vl_Lista = 15.00 });


            return produtos;
        }

        public IList<TabelaPreco> GetTabelaPreco()
        {

            var tabPreco = new List<TabelaPreco>();

            tabPreco.Add(new TabelaPreco { id = 1, ds_descricao = "TABELA_NORMAL", id_codigo = "1", vl_descontomaximo = 5.00, vl_valor = 10.00 });
            tabPreco.Add(new TabelaPreco { id = 2, ds_descricao = "TABELA_PROMOÇÃO", id_codigo = "2", vl_descontomaximo = 7.00, vl_valor = 12.00 });

            return tabPreco;
        }

        public IList<ProdutoTabelaPreco> GetProdTabelaPreco()
        {

            var prodTabPreco = new List<ProdutoTabelaPreco>();

            prodTabPreco.Add(new ProdutoTabelaPreco { id = 1, id_produto = 1, id_tabpreco = 1, vl_Valor = 5.25 });
            prodTabPreco.Add(new ProdutoTabelaPreco { id = 2, id_produto = 2, id_tabpreco = 1, vl_Valor = 11.75 });
            prodTabPreco.Add(new ProdutoTabelaPreco { id = 3, id_produto = 3, id_tabpreco = 2, vl_Valor = 15.00 });

            return prodTabPreco;
        }
    }
}