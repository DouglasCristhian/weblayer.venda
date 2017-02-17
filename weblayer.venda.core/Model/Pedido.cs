using System;
using SQLite;

namespace weblayer.venda.core.Model
{
    [Table("Pedidos")]
    public class Pedido
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [MaxLength(20), NotNull]
        public string id_codigo { get; set;}

        [NotNull]
        public virtual DateTime? dt_emissao { get; set;}

        [MaxLength(20), NotNull]
        public int id_cliente { get; set;}

        [MaxLength(200), NotNull]
        public string ds_cliente { get; set;}

        [MaxLength(20), NotNull]
        public int id_vendedor { get; set;}

        [MaxLength(200), NotNull]
        public string ds_vendedor { get; set;}

        [MaxLength(20)]
        public double vl_total { get; set;}

        [MaxLength(200)]
        public string ds_observacao { get; set; }

        [MaxLength(200)]
        public string ds_MsgPedido { get; set; }

        [MaxLength(200)]
        public string ds_MsgNF { get; set; }

        public int fl_status { get; set; }
    }
}