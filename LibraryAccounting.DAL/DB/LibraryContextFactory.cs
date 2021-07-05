using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.DB
{
    /// <summary>
    /// Класс фабрика для внедрения зависимости во время исполнения
    /// </summary>
    public class LibraryContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
    {
        /// <summary>
        /// Созадине контекста базы данных
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Контекст базы данных</returns>
        public LibraryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
            optionsBuilder.UseNpgsql(DbConfig.GetConnectionString());

            return new LibraryDbContext(optionsBuilder.Options);
        }
    }
}

