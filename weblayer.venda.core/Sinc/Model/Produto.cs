namespace weblayer.venda.core.Sinc.Model
{
    public class Produto
    {
        public int id { get; set; }
        public string id_codigo { get; set; }
        public string ds_nome { get; set; }
        public string ds_unimedida { get; set; }
        public double vl_Lista { get; set; }
        public double vl_Venda { get; set; }
    }
}