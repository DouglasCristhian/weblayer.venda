using SQLite;

namespace weblayer.venda.core.Model
{
    [Table("Produto")]
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [MaxLength(12), NotNull]
        public string id_codigo { get; set; }

        [MaxLength(200), NotNull]
        public string ds_nome { get; set; }

        [MaxLength(20), NotNull]
        public string ds_unimedida { get; set; }

        public double vl_Lista { get; set; }

        public double vl_Venda { get; set; }
    }
}