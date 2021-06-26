using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.Properties.DB
{
    public class DbConfig
    {
        public static string GetConnectionString()
        {

            string server = Environment.GetEnvironmentVariable("DB_HOST") == null 
                ? "localhost" : Environment.GetEnvironmentVariable("DB_HOST");
            var name = Environment.GetEnvironmentVariable("DB_NAME") == null ?
                "library-accounting_db_1" : Environment.GetEnvironmentVariable("DB_NAME");
            var user = Environment.GetEnvironmentVariable("DB_USER") == null ?
                "admin" : Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") == null ?
                "123" : Environment.GetEnvironmentVariable("DB_PASSWORD");

            var connection = $"Username={user};Password={password};Server={server};Database={name}";
            return connection;
        }
    }
}
