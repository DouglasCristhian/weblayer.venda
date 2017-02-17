using SQLite;
using weblayer.venda.core.Model;
using Environment = System.Environment;

namespace weblayer.venda.core.Dal
{
    public class Database : SQLiteConnection
    {
        private static readonly string Path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "database.db");

        public Database(string databasePath, bool storeDateTimeAsTicks = true) : base(databasePath, storeDateTimeAsTicks)
        {
        }

        public Database(string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = true) : base(databasePath, openFlags, storeDateTimeAsTicks)
        {
        }


        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(Path, true);
        }

        public static void Initialize()
        {
            CreateDatabase(GetConnection());

            //new ProdutoRepository().MakeDataMock();
            //new TabelaPrecoRepository().MakeDataMock();
            //new ClienteRepository().MakeDataMock();
            new PedidoRepository().MakeDataMock();
            //new ProdutoTabelaPrecoRepository().MakeDataMock();
            //new PedidoItemRepository().MakeDataMock();

        }

        private static void CreateDatabase(SQLiteConnection connection)
        {

            CreateTables(connection);
        }

        private static void CreateTables(SQLiteConnection connection)
        {
            using (connection)
            {
                connection.CreateTable<Cliente>();
                connection.CreateTable<Produto>();
                connection.CreateTable<TabelaPreco>();
                connection.CreateTable<PedidoItem>();
                connection.CreateTable<Pedido>();
                connection.CreateTable<ProdutoTabelaPreco>();
            }
        }
    }
}