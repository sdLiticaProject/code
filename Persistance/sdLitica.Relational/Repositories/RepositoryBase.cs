using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdLitica.Relational.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, IEntity
    {
        private readonly DbContext _context;

        protected DbSet<T> Entity { get; }

        public RepositoryBase(DbContext context)
        {
            _context = context;
            Entity = _context.Set<T>();
        }

        public void Add(T entity)
        {
            Entity.Add(entity);            
        }

        public void Update(T entity)
        {
            Entity.Update(entity);            
        }

        public void AddRange(IEnumerable<T> entity)
        {
            Entity.AddRange(entity);            
        }

        public void Delete(T entity)
        {
            Entity.Remove(entity);            
        }

        public T GetById(Guid id)
        {
            return Entity.SingleOrDefault(e => e.Id == id);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await Entity.SingleOrDefaultAsync(e => e.Id == id);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
