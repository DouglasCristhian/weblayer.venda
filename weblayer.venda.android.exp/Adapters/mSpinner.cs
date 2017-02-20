namespace weblayer.venda.android.exp.Adapters
{
    class mSpinner
    {
        public int id;
        public string ds;

        public mSpinner(int idprod, string dsprod)
        {
            id = idprod;
            ds = dsprod;
        }

        public int Id()
        {
            return id;
        }

        public override string ToString()
        {
            return ds;
        }
    }
}