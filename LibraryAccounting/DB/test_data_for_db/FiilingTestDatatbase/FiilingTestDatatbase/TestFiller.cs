using System;
using System.Collections.Generic;
using LibraryAccounting.Entities;
using System.Linq;
using Medallion;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Data.Common;

namespace LibraryAccounting.DB.test_data_for_db
{
    public class TestFiller
    {
        public static void FillReservations(LibraryAccounting.DB.LibraryDbContext context)
        {
            Random rnd = new Random();
            var reservedBooks = (from b in context.Books
                                 where b.StatusId == 2
                                 select b.Id).ToList();
            reservedBooks.AddRange(reservedBooks);
            reservedBooks.AddRange(reservedBooks);
            reservedBooks.AddRange(reservedBooks);
            reservedBooks.Shuffle();
            reservedBooks = reservedBooks.Take(rnd.Next(reservedBooks.Count)).ToList();
            reservedBooks.ForEach(r =>
            {
                context.Books.Where(b => b.Id == r).FirstOrDefault().StatusId = 1;
                context.SaveChanges();
            });
            var employees = (from r in context.Emloyees select r.Id).ToList();
            employees.Shuffle();
            employees = employees.Take(reservedBooks.Count).ToList();

            for (int i = 0; i < reservedBooks.Count; i++)
            {
                rnd = new Random();
                DateTime start = new DateTime(2015, 1, 1);
                int range = (DateTime.Today - start).Days;
                var addDays = rnd.Next(range);
                start = start.AddDays(addDays);
                DateTime stop = start.AddDays(14);
                context.Reservations.Add(new Entities.Reservations
                {
                    ReaderId = employees[i],
                    BookId = reservedBooks[i],
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

        public static void FillUserLogins(LibraryAccounting.DB.LibraryDbContext context)
        {
            byte[] globalSalt = Encoding.ASCII.GetBytes("Dear Math, grow up and solve your own problems");
            var users = context.Emloyees.Where(u => u.FirstName != "Dev-adm" || u.FirstName != "Dev-usr").Select(u => new { u.Id, u.FirstName, u.LastName }).ToList();

            foreach (var user in users)
            {
                var random = new RNGCryptoServiceProvider();
                var slt = new byte[1024];
                random.GetNonZeroBytes(slt);
                var cryptoAlgGlobal = new HMACSHA512(globalSalt);
                var cryptoAlg = new HMACSHA512(slt);

                StringBuilder str_build = new StringBuilder();
                Random rnd = new Random();
                int lenght = rnd.Next(8, 15);
                char letter;

                for (int i = 0; i < lenght; i++)
                {
                    int shift = Convert.ToInt32(Math.Floor(25 * rnd.NextDouble()));
                    letter = Convert.ToChar(shift + 65);
                    str_build.Append(letter);
                }

                var saltedPassword = cryptoAlg.ComputeHash(cryptoAlgGlobal.ComputeHash(Encoding.ASCII.GetBytes(str_build.ToString())));
                var login = new DbLogin()
                {
                    EmployeeId = user.Id,
                    UserName = user.FirstName + " " + user.LastName,
                    Password = Convert.ToBase64String(saltedPassword),
                    Salt = slt
                };
                context.Logins.Add(login);
                context.SaveChanges();
            }
        }
        public static void RestoreBooks(LibraryDbContext context)
        {
            var books = (from res in context.Reservations
                         join book in context.Books on res.BookId equals book.Id
                         where book.StatusId == 1 && res.ReturningFlag == true
                         select book).ToList();
            books.ForEach(b => b.StatusId = 2);
            books.Select(b => context.Entry(b).State = EntityState.Modified);
            context.SaveChanges();

            books = (from book in context.Books
                     where book.StatusId == 1
                     select book).ToList();
            var employees = (from r in context.Emloyees select r.Id).ToList();
            employees.Shuffle();
            employees = employees.Take(books.Count).ToList();

            for (int i = 0; i < books.Count; i++)
            {
                Random rnd = new Random();
                DateTime start = DateTime.Now;
                int range = (DateTime.Today.AddDays(30) - start).Days;
                var addDays = rnd.Next(range);
                start = start.AddDays(addDays);
                DateTime stop = start.AddDays(14);
                context.Reservations.Add(new Entities.Reservations
                {
                    ReaderId = employees[i],
                    BookId = books[i].Id,
                    ReservationDate = start,
                    ReturnDate = stop,
                    ReturningFlag = stop <= DateTime.Today
                });
                context.SaveChanges();
            }
        }
    }
}
