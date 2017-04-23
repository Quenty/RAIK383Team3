using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PracticalWerewolf.Stores.Interfaces
{
    public interface IEntityStore<T> where T : class
    {
        IQueryable<T> AsQueryable();
        IQueryable<T> AsNoTracking();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> Find(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        T Find(object id);
        T Single(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        T First(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        void Delete(T entity);
        void Insert(T entity);
        void Update(T entity);
        DbEntityEntry<T> GetEntry(T entity);
        void Attach(T entity);
        T Refresh(T entity);
    }
}