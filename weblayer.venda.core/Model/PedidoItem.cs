using SQLite;

namespace weblayer.venda.core.Model
{
    [Table("PedidoItem")]
    public class PedidoItem
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [MaxLength(20), NotNull]
        public int id_pedido { get; set; }

        [MaxLength(15), NotNull]
        public int id_produto { get; set; }

        [MaxLength(200), NotNull]
        public string ds_produto { get; set; }

        [MaxLength(20)]
        public double vl_Lista { get; set; }

        [MaxLength(20)]
        public double vl_Desconto { get; set; }

        [MaxLength(20)]
        public double vl_Venda { get; set; }

        [MaxLength(20), NotNull]
        public int nr_quantidade { get; set; }
    }
}