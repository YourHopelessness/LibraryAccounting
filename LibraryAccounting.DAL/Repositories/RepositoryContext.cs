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
        private readonly BaseLibraryContext _db;
        private bool disposed = false;
        private EntityRepository<Changes> changes;
        private EntityRepository<Reservations> reservations;
        private EntityRepository<Books> books;
        private EntityRepository<Employees> employees;
        private EntityRepository<BooksStatuses> booksStatuses;
        private EntityRepository<DbLogin> logins;
        private EntityRepository<UserRoles> userroles;
        private EntityRepository<Roles> roles;

        /// <summary>
        /// Конструктор
        /// </summary>
        public LibraryUOW(BaseLibraryContext context)
        {
            _db = context;
        }

        /// <summary>Синглтон репозитория таблицы книг</summary>
        public EntityRepository<Books> Books { get => books ??= new EntityRepository<Books>(_db); }

        /// <summary> Синглтон репозитория таблицы бронирования</summary>
        public EntityRepository<Reservations> Reservations { get => reservations ??= new EntityRepository<Reservations>(_db); }

        /// <summary> Синглтон репозитория таблицы изменений</summary>
        public EntityRepository<Changes> Changes { get => changes ??= new EntityRepository<Changes>(_db); }

        /// <summary>Синглтон репозитория справочника сотрудников</summary>
        public EntityRepository<Employees> Employees { get => employees ??= new EntityRepository<Employees>(_db); }

        /// <summary>Синглтон репозитория справочника статусов книг</summary>
        public EntityRepository<BooksStatuses> BooksStatuses { get => booksStatuses ??= new EntityRepository<BooksStatuses>(_db); }

        /// <summary>Синглтон репозитория данных аутентификации</summary>
        public EntityRepository<DbLogin> Logins { get => logins ??= new EntityRepository<DbLogin>(_db); }

        /// <summary>Синглтон репозитория ролей пользователей</summary>
        public EntityRepository<UserRoles> UserRoles { get => userroles ??= new EntityRepository<UserRoles>(_db); }

        /// <summary>Синглтон репозитория ролей</summary>
        public EntityRepository<Roles> Roles { get => roles ??= new EntityRepository<Roles>(_db); }

        /// <summary>Сохрание в базу данных</summary>
        public async Task Save()
        {
            await _db.SaveChangesAsync();
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
