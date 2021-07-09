using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using LibraryAccounting.DAL.DB;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace LibraryAccounting.DAL.Repositories
{
    /// <summary>
    /// Класс универсального репозитория для любой сущности
    /// </summary>
    /// <typeparam name="Entity">Тип сущности, передаваемый при инициализации, для которого создается репозиторий</typeparam>
    public class EntityRepository<Entity> where Entity : class
    {
        private readonly BaseLibraryContext _dbContext; // текущий контекст для работы с бд
        private readonly DbSet<Entity> _entitySet; // текущий набор сущностей

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст для работы с базой данных</param>
        public EntityRepository(BaseLibraryContext context)
        {
            _dbContext = context;
            _entitySet = _dbContext.Set<Entity>();
        }
        /// <summary>
        /// Функция для поиска кортежей в базе данных по заданным условиям отбора и сортировки
        /// </summary>
        /// <param name="filter">Лямбда-параметр для фильтрации </param>
        /// <param name="orderBy">Лямбда для установления порядка сортировки</param>
        /// <param name="properties">Массив параметров, по которым производиться выборка</param>
        /// <exception>Генерирует исключение EntityException, если параметр выборки указан неверно</exception>
        /// <returns>Вовзращает набор кортежей по условию поиска</returns>
        public virtual IEnumerable<Entity> Get(Expression<Func<Entity, bool>> filter = null,
                                               Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
                                               string[] properties = null)
        {
            IQueryable<Entity> query = _entitySet; // текущий запрос

            if (filter != null)
            {
                query = query.Where(filter); // поисков объекту по фильтру
            }

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    try
                    {
                        query = query.Include(property); // поиск объектов по параматрам
                    }
                    catch (ArgumentException)
                    {
                        throw new EntityException($"В сущности {typeof(Entity)} нет параметра {property}");
                    }
                }
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList(); // сортировка по параметру
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// CRUD операция вставки в базу данных
        /// </summary>
        /// <param name="entity">Объект, представляющий кортеж, который вносится в базу данных</param>
        public virtual void Insert(Entity entity)
        {
            _entitySet.Add(entity);
        }

        /// <summary>
        /// CRUD операция удаления
        /// </summary>
        /// <param name="entityToDelete">Удалаяемый объект - кортеж из бд</param>
        public virtual void Delete(Entity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _entitySet.Attach(entityToDelete);
            }
            _entitySet.Remove(entityToDelete);
        }
        /// <summary>
        /// CRUD операция обновления
        /// </summary>
        /// <param name="entityToUpdate">Обновляемый объект - кортеж</param>
        public virtual void Update(Entity entityToUpdate)
        {
            _entitySet.Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
