using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace MVCWeb.Cores.IRepositories
{
    public interface IGenericRepository<T> : IWebAppRepository where T : class
    {
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(int id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(T entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<T> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(T entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Attach entity to be tracked by EF
        /// </summary>
        /// <param name="entity">Entity</param>
        void Attach(T entity);

        /// <summary>
        /// Gets entity entry
        /// </summary>
        /// <param name="entity">Entity</param>
        DbEntityEntry<T> Entry(T entity);

        /// <summary>
        /// Excute sql store procedure.
        /// </summary>
        /// <typeparam name="TElement">The type of element that map to the return values</typeparam>
        /// <param name="name">The name of store procedure</param>
        /// <param name="parameters">The parameters of store procedure</param>
        /// <returns>The return values</returns>
        DbRawSqlQuery<TElement> ExcuteStoreProcedure<TElement>(string name, params object[] parameters) where TElement : class;

        /// <summary>
        /// Excute sql scalar store procedure.
        /// </summary>
        /// <typeparam name="TElement">The type of element that map to the return values</typeparam>
        /// <param name="name">The name of store procedure</param>
        /// <param name="parameters">The parameters of store procedure</param>
        /// <returns>The return values</returns>
        DbRawSqlQuery<TElement> ExcuteScalarStoreProcedure<TElement>(string name, params object[] parameters);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
    }
}
