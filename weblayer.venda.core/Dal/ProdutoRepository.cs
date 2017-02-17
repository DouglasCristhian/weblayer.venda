using System;
using System.Collections.Generic;
using System.Linq;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Dal
{
    public class ProdutoRepository
    {
        public string Mensage { get; set; }

        public Produto Get(int id)
        {
            return Database.GetConnection().Table<Produto>().Where(x => x.id == id).FirstOrDefault();
        }

        public void Save(Produto entidade)
        {

            try
            {
                if (entidade.id > 0 && Get(entidade.id) != null)
                    Database.GetConnection().Update(entidade);
                else
                    Database.GetConnection().Insert(entidade);
            }
            catch (Exception e)
            {
                Mensage = $"Falha ao Inserir a entidade {entidade.GetType()}. Erro: {e.Message}";
            }
        }

        public void Delete(Produto entidade)
        {
            Database.GetConnection().Delete(entidade);
        }

        public IList<Produto> List()
        {
            return Database.GetConnection().Table<Produto>().ToList();
        }

        public IList<Produto> ListFiltro(string filtro)
        {
            return Database.GetConnection().Table<Produto>().Where(x => x.ds_nome.StartsWith(filtro)).ToList();
        }

        //public string Prod(Produto entidade)
        //{
        //    return Database.GetConnection().Table<Produto>().Where(x => entidade.ds_nome == x.ds_nome).ToList().ToString();
        //}

        //public string DescProd(int id)
        //{
        //    return Database.GetConnection().Table<Produto>().Where(x => x.id == id).ToString();
        //}

        //public void MakeDataMock()
        //{
        //    if (List().Count > 0)
        //        return;

        //    Save(new Produto() { id_codigo = "1111", ds_nome = "LAPIS DE COR AMARELO", ds_unimedida = "CX", id_tabpreco = 1, /*vl_Valor = 1.00*/ vl_Lista = 1.00 });
        //    Save(new Produto() { id_codigo = "2222", ds_nome = "LAPIS DE COR VERMELHO", ds_unimedida = "PCT", id_tabpreco = 2, /*vl_Valor = 1.00*/ vl_Lista = 1.00 });
        //    Save(new Produto() { id_codigo = "3333", ds_nome = "LAPIS DE COR AZUL", ds_unimedida = "CX", id_tabpreco = 2, /*vl_Valor = 1.00 */ vl_Lista = 1.00 });
        //    Save(new Produto() { id_codigo = "4444", ds_nome = "LAPIS DE COR PRETO", ds_unimedida = "UN", id_tabpreco = 1, /*vl_Valor = 1.00*/ vl_Lista = 1.00 });
        //}

    }
}