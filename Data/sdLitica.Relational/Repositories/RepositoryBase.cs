using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdLitica.Relational.Repositories
{
    /// <summary>
    /// Abstract class that provides CRUD operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, IEntity
    {
        private readonly DbContext _context;

        /// <summary>
        /// DbSet of the entity T, it is used to access the table that corresponds to T
        /// </summary>
        protected DbSet<T> Entity { get; }

        /// <summary>
        /// Creates this class to a provided DbContext
        /// </summary>
        /// <param name="context"></param>
        public RepositoryBase(DbContext context)
        {
            _context = context;
            Entity = _context.Set<T>();
        }

        /// <summary>
        /// Add an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            Entity.Add(entity);            
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            Entity.Update(entity);            
        }

        /// <summary>
        /// Add a range of entities
        /// </summary>
        /// <param name="entity"></param>
        public void AddRange(IEnumerable<T> entity)
        {
            Entity.AddRange(entity);            
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            Entity.Remove(entity);            
        }

        /// <summary>
        /// Get an entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(Guid id)
        {
            return Entity.SingleOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Get an entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await Entity.SingleOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Save changes in DbContext. 
        /// This method run commands in the database (e.g. runs a transaction).
        /// </summary>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Save changes in DbContext. 
        /// This method run commands in the database (e.g. runs a transaction).
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
