using System;
using System.Collections.Generic;
using LibraryAccounting.Entities;
using System.Linq;
using Medallion;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryAccounting.DB.test_data_for_db
{
    public class TestFiller
    {
        public static void FillReservations(LibraryAccounting.DB.LibraryDbContext context)
        {
            var reservedBooks = (from b in context.Books
                                 where b.StatusId == 2
                                 select b.Id).ToList();
            var employees = (from r in context.Emloyees select r.Id).ToList();
            employees.Shuffle();
            employees = employees.Take(reservedBooks.Count).ToList();

            for (int i = 0; i < reservedBooks.Count; i++)
            {
                Random rnd = new Random();
                DateTime start = new DateTime(2015, 1, 1);
                int range = (DateTime.Today - start).Days;
                var addDays = rnd.Next(range);
                start = start.AddDays(addDays);
                DateTime stop = start.AddDays(14);
                context.Reservations.Add(new Entities.Reservations
                {
                    ReaderId = reservedBooks[i],
                    BookId = employees[i],
                    ReservationDate = start,
                    ReturnDate = stop,
                    ReturningFlag = stop <= DateTime.Today
                });
                context.SaveChanges();
            }
        }

        public static void FillChanges(LibraryAccounting.DB.LibraryDbContext context)
        {
            Random random = new Random();
            var changeNum = random.Next(15, 100);
            var users = (from ur in context.UserRoles where ur.EmployeeId != null select ur).ToList();
            var books = (from b in context.Books select b.Id).ToList();
            users.Shuffle();
            books.Shuffle();
            books = books.Take(changeNum).ToList();
            users = users.Take(changeNum).ToList();
            users.ForEach(c => c.RoleId = 1);
            users.Select(u => context.Entry(u).State = EntityState.Modified);
            context.SaveChanges();

            for (int i = 0; i < changeNum; i++)
            {
                random = new Random();
                DateTime start = new DateTime(2018, 1, 1);
                int range = (DateTime.Today - start).Days;
                start = start.AddDays(random.Next(range));
                context.Changes.Add(new Entities.Changes
                {
                    ChangemakerId = (from e in context.Emloyees where e.Id == users[i].EmployeeId select e.Id).FirstOrDefault(),
                    BookId = books[i],
                    ChangeDate = start,
                    Comment = ""
                }) ;
                context.SaveChanges();
            }
        }
     
    }
}
