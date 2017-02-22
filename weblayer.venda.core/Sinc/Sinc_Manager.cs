

using System.Collections.Generic;
using weblayer.venda.android.Sinc;
using weblayer.venda.core.Bll;
using weblayer.venda.core.Sinc.Model;

namespace weblayer.venda.core.Sinc
{
    public class Sinc_Manager
    {
        public void Sincronizar()
        {

            var webservice = new WebServiceMock();

            //Tabela de clientes - pegar os dados do webservice e sincronizar na base local.
            Sincronizar_Cliente(webservice.GetClientes());
            Sincronizar_Produto(webservice.GetProdutos());
            Sincronizar_TabelaPreco(webservice.GetTabelaPreco());
            Sincronizar_ProdutoTabelaPreco(webservice.GetProdTabelaPreco());

            //System.Threading.Thread.Sleep(3000);

        }

        private void Sincronizar_Cliente(IList<Cliente> Clientes)
        {

            var baselocal = new Cliente_Manager();

            foreach (var item in Clientes)
            {

                var clientedatabase = new core.Model.Cliente
                {
                    id = item.id,
                    ds_Cnpj = item.ds_Cnpj,
                    ds_NomeFantasia = item.ds_NomeFantasia,
                    ds_RazaoSocial = item.ds_RazaoSocial,
                    id_Codigo = item.id_Codigo,
                    id_tabelapreco = item.id_tabelapreco
                };


                baselocal.Save(clientedatabase);
            }
        }

        private void Sincronizar_Produto(IList<Produto> Produtos)
        {

            var baselocal = new Produto_Manager();

            foreach (var item in Produtos)
            {

                var produtodatabase = new core.Model.Produto
                {
                    id = item.id,
                    ds_nome = item.ds_nome,
                    ds_unimedida = item.ds_unimedida,
                    id_codigo = item.id_codigo,
                    //id_tabpreco = item.id_tabpreco,
                    vl_Lista = item.vl_Lista,
                };

                baselocal.Save(produtodatabase);
            }
        }

        private void Sincronizar_TabelaPreco(IList<TabelaPreco> TabelasPreco)
        {

            var baselocal = new TabelaPreco_Manager();

            foreach (var item in TabelasPreco)
            {

                var tabelaprecodatabase = new core.Model.TabelaPreco
                {
                    id = item.id,
                    ds_descricao = item.ds_descricao,
                    id_codigo = item.id_codigo,
                    //vl_descontomaximo = item.vl_descontomaximo,
                    //vl_valor = item.vl_descontomaximo,
                };

                baselocal.Save(tabelaprecodatabase);
            }
        }

        private void Sincronizar_ProdutoTabelaPreco(IList<ProdutoTabelaPreco> ProdutosTabelaPreco)
        {

            var baselocal = new ProdutoTabelaPreco_Manager();

            foreach (var item in ProdutosTabelaPreco)
            {

                var produtotabprecodatabase = new core.Model.ProdutoTabelaPreco
                {
                    id = item.id,
                    id_produto = item.id_produto,
                    id_tabpreco = item.id_tabpreco,
                    vl_Valor = item.vl_Valor,
                };

                baselocal.Save(produtotabprecodatabase);
            }
        }
    }
}