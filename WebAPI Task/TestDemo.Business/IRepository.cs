using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace TestDemo.Business
{
  public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        Task<ICollection<TEntity>> GetAllAsync();

        TEntity Get(int id);
        Task<TEntity> GetAsync(int id);

        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);

        TEntity Update(TEntity entity, object key);
        Task<TEntity> UpdateAsync(TEntity entity, object key);

        void Delete(int id);
        Task<int> DeleteAsync(int id);

        void Save();
        Task<int> SaveAsync();
    }
}
