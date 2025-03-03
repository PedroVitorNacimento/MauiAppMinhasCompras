using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Holpers
{
    public class SQLiteDatabasehelper
    {
        //propriedade que armazena a conexão
        readonly SQLiteAsyncConnection conn;
        public SQLiteDatabasehelper(string path)
        {
            conn = new SQLiteAsyncConnection(path);
            conn.CreateTableAsync<Produto>().Wait();
        }
        public Task<int> Insert(Produto p) 
        {
            return conn.InsertAsync(p);
        }
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE produto SET Descricao=?, quantidade=?, preco=? WHERE id=?";
            return conn.QueryAsync<Produto>(sql,p.Descricao, p.Quantidade, p.Preco, p.Id);
        }
        public Task<int> Delete(int id)
        {
           return conn.Table<Produto>().DeleteAsync(i => i.Id == id)
        }

        public Task<List<Produto>> GetAll() 
        {
            return conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> search(string q) 
        {
            string sql = "SELECT * produto WHERE descricao LIKE '%"+ q + "%'";
            return conn.QueryAsync<Produto>(sql);
        }
    }
}
