using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sdLitica.Entities.Abstractions
{
    /// <summary>
    /// Interface that provides CRUD operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryBase<T> where T : class, IEntity
    {
        /// <summary>
        /// Add an entity
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);
        /// <summary>
        /// Add a range of entities
        /// </summary>
        /// <param name="entity"></param>
        void AddRange(IEnumerable<T> entity);
        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
        /// <summary>
        /// Get an entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(Guid id);
        /// <summary>
        /// Get an entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(Guid id);
        /// <summary>
        /// Save changes in DbContext. 
        /// This method run commands in the database (e.g. runs a transaction).
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Save changes in DbContext. 
        /// This method run commands in the database (e.g. runs a transaction).
        /// </summary>
        Task SaveChangesAsync();
    }
}
