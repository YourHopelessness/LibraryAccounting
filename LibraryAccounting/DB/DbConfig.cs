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
            string connStr = "";
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                connStr = appSettings["LibraryContext"];
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings\n Load standart data connection");
                connStr = $"Username=admin;Password=123;Server=localhost;Database=library-accounting_db_1";
            }

            return connStr;
        }
    }
}
