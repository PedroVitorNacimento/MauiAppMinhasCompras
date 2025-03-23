using MauiAppMinhasCompras.Helpers;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabasehelper? _db;
        public static SQLiteDatabasehelper Db 
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "banco_sqlite_compras.db3"
                        );

                    _db = new SQLiteDatabasehelper(path);
                }
                return _db;

            }
        }
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new NavigationPage( new Views.ListaProduto());
        }
    }
}
