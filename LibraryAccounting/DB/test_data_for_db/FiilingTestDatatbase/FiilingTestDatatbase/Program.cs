using Microsoft.EntityFrameworkCore;
using System;

namespace FiilingTestDatatbase
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContextOptionsBuilder<LibraryAccounting.DB.LibraryDbContext> builder = new DbContextOptionsBuilder<LibraryAccounting.DB.LibraryDbContext>();
            builder.UseNpgsql($"Username=admin;Password=123;Server=localhost;Database=library-accounting_db_1");
            LibraryAccounting.DB.LibraryDbContext dbContext = new LibraryAccounting.DB.LibraryDbContext(builder.Options);

            var c = dbContext.Reservations.CountAsync().GetAwaiter().GetResult();
            if (c == 0)
            {
                Console.WriteLine("Start filling reservations table...");
                LibraryAccounting.DB.test_data_for_db.TestFiller.FillReservations(dbContext);
                Console.WriteLine("Reservations table are filled...");
            }
            c = dbContext.Changes.CountAsync().GetAwaiter().GetResult();
            if (c == 0)
            {
                Console.WriteLine("Start filling changes table...");
                LibraryAccounting.DB.test_data_for_db.TestFiller.FillChanges(dbContext);
                Console.WriteLine("Changes table are filled...");
            }
            
        }
    }
}
