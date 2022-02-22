using System;
using System.Linq;

namespace AppleStore.Models.Repositories
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> All();

        IQueryable<TEntity> AllAsNoTracking();

        TEntity GetById(string id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        int SaveChanges();
    }
}
