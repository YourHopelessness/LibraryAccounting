using System;
using System.Collections.Generic;
using LibraryAccounting.DAL.Entities;
using LibraryAccounting.DAL.DB;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAccounting.DAL.Repositories
{
   /// <summary>
   /// Класс паттерна Unit of Work
   /// </summary>
    public class LibraryUOW
    {
        private readonly LibraryDbContext _db;
        private bool disposed = false;
        private EntityRepository<Changes> changes;
        private EntityRepository<Reservations> reservations;
        private EntityRepository<Books> books;
        private EntityRepository<Employees> employees;

        /// <summary>
        /// Конструктор
        /// </summary>
        public LibraryUOW(LibraryDbContext context)
        {
            _db = context;
        }
        /// <summary>
        /// Синглтон репозитория таблицы книг
        /// </summary>
        public EntityRepository<Books> Books 
        {
            get
            {
                if (books == null)
                {
                    books = new EntityRepository<Books>(_db);
                }
                return books;
            }
        }
        /// <summary>
        /// Синглтон репозитория таблицы бронирования
        /// </summary>
        public EntityRepository<Reservations> Reservations 
        {
            get
            {
                if (reservations == null)
                {
                    reservations = new EntityRepository<Reservations>(_db);
                }
                return reservations;
            }
        }
        /// <summary>
        /// Синглтон репозитория таблицы изменений
        /// </summary>
        public EntityRepository<Changes> Changes 
        { 
            get
            {
                if (changes == null)
                {
                    changes = new EntityRepository<Changes>(_db);
                }
                return changes;
            }
        }
        /// <summary>
        /// Синглтон репозитория справочника сотрудников
        /// </summary>
        public EntityRepository<Employees> Employees
        {
            get
            {
                if (employees == null)
                {
                    employees = new EntityRepository<Employees>(_db);
                }
                return employees;
            }
        }

        /// Сохрание в базу данных
        public void Save()
        {
            _db.SaveChanges();
        }
        /// <summary>
        /// Функция для уничтожения репозиториев
        /// </summary>
        /// <param name="disposing">Флаг уничтожения</param>
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                disposed = true;
            }
        }
        /// <summary>
        /// Уничтожение объектов
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
