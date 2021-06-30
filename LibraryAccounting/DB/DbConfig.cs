using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace LibraryAccounting.DB
{
    public class DbConfig
    {
        public static string GetConnectionString()
        {
            string connStr = null;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                connStr = appSettings["ConnectionString:LibraryContext"];
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings\n Load standart data connection");
            }

            return connStr ?? $"Username=admin;Password=123;Server=localhost;Port=5432;Database=library-accounting_db_1";
        }
    }
}
