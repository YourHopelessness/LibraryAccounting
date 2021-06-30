using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAccounting.DB.test_data_for_db
{
    public class TestFiller
    {

        public static void Fill(LibraryDbContext context)
        {
            var reservation = from b in context.Books
                              join r in context.Emloyees on b.StatusId equals 2
                              select new
                              {
                                  readerId = r.Id,
                                  bookId = b.Id
                              };
            foreach(var res in reservation)
            {
                Random rnd = new Random();
                DateTime start = new DateTime(2005, 1, 1);
                int range = (DateTime.Today - start).Days;
                start = start.AddDays(rnd.Next(range));
                DateTime stop = start.AddDays(14);
                context.Reservations.Add(new Entities.Reservations 
                                            {
                                                ReaderId = res.readerId,
                                                BookId = res.bookId, 
                                                ReservationDate = start, 
                                                ReturnDate = stop,
                                                ReturningFlag = stop <= DateTime.Today
                                            }); 
            }
            context.SaveChangesAsync();
        }
    }
}
