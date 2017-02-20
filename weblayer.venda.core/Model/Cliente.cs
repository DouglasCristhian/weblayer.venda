using SQLite;

namespace weblayer.venda.core.Model
{
    [Table("Cliente")]
    public class Cliente
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [MaxLength(60)]
        public string id_Codigo { get; set; }

        public int id_tabelapreco { get; set; }

        [MaxLength(200), NotNull]
        public string ds_RazaoSocial { get; set; }

        [MaxLength(200), NotNull]
        public string ds_NomeFantasia { get; set; }

        [MaxLength(20)]
        public string ds_Cnpj { get; set; }
    }
}