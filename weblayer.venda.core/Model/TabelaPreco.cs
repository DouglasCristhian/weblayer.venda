using SQLite;

namespace weblayer.venda.core.Model
{
    [Table("TabelaPreco")]
    public class TabelaPreco
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [MaxLength(20)]
        public string id_codigo { get; set; }

        [MaxLength(200)]
        public string ds_descricao { get; set; }

        public double vl_valor { get; set; }

        public double vl_descontomaximo { get; set; }
    }
}