using System;
using TestDemo.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TestDemo.Business
{
    public class Respository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDataContext _appDataContext;
        private DbSet<TEntity> _entities;

        public Respository(AppDataContext appDataContext)
        {
            _appDataContext = appDataContext;
            _entities = appDataContext.Set<TEntity>();
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _entities;
        }

        public TEntity Get(int id)
        {
            return _entities.Find(id);
        }

        public Task<TEntity> GetAsync(int id)
        {
            return _entities.FindAsync(id);
        }

        public TEntity Add(TEntity entity)
        {
            _entities.Add(entity);
            _appDataContext.SaveChanges();
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            _entities.Add(entity);
            await _appDataContext.SaveChangesAsync();
            return entity;
        }

        public TEntity Update(TEntity entity, object key)
        {
            if (entity == null)
                return null;
            TEntity exist = _entities.Find(key);
            if (exist != null)
            {
                _appDataContext.Entry(exist).CurrentValues.SetValues(entity);
                _appDataContext.SaveChanges();
            }
            return exist;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, object key)
        {
            if (entity == null)
                return null;
            TEntity exist = await _entities.FindAsync(key);
            if (exist != null)
            {
                _appDataContext.Entry(exist).CurrentValues.SetValues(entity);
                await _appDataContext.SaveChangesAsync();
            }
            return exist;
        }

        public void Delete(int id)
        {
            var entity = _entities.Find(id);
            if (entity == null)
                throw new Exception("Invalid Id");

            _entities.Remove(entity);
            _appDataContext.SaveChanges();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await _entities.FindAsync(id);
            if (entity == null)
                throw new Exception("Invalid Id");

            _entities.Remove(entity);
            return await _appDataContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _appDataContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Save()
        {
            _appDataContext.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _appDataContext.SaveChangesAsync();
        }

    }
}
