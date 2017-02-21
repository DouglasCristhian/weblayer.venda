using System;
using System.Collections.Generic;
using weblayer.venda.core.Dal;
using weblayer.venda.core.Model;

namespace weblayer.venda.core.Bll
{
    public class Cliente_Manager
    {
        public string Mensagem;

        public Cliente Get(int id)
        {
            return new ClienteRepository().Get(id);
        }

        public IList<Cliente> GetClientes(string filtro)
        {
            return new ClienteRepository().ListFiltro(filtro);
        }

        public IList<Cliente> GetCli(string filtro)
        {
            return new ClienteRepository().ListFiltro(filtro);
        }

        public void Save(Cliente obj)
        {
            var erros = "";

            //regras....
            if (obj.id_Codigo.Length < 0)
                erros = erros + "\n O c�digo do cliente � inv�lido! Ele deve ter no m�nimo 1 caracter!";

            if (obj.ds_NomeFantasia.Length < 10)
                erros = erros + "\n A descri��o do cliente deve ter no m�nimo 10 caracteres!";

            //TODO: Devidas exce��es

            if (erros.Length > 0)
                throw new Exception(erros);

            var Repository = new ClienteRepository();
            Repository.Save(obj);

            Mensagem = $"Cliente {obj.ds_RazaoSocial} atualizado com sucesso";
        }

        public void Delete(Cliente obj)
        {
            var Repository = new ClienteRepository();
            Repository.Delete(obj);

            Mensagem = $"Cliente {obj.ds_RazaoSocial} exclu�do com sucesso";
        }
    }
}