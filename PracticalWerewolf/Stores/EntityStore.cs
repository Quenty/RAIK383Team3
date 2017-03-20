using PracticalWerewolf.Stores.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PracticalWerewolf.Stores
{
    // https://github.com/icotting/SETwitter/blob/9bf60ffe26cacee4b44b4a9eae48156bf2145949/ASP.NET/Twitter_Shared/Data/EntityRepository.cs
    public abstract class EntityStore<T> : IEntityStore<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly IDbSetFactory _dbSetFactory;

        public EntityStore(IDbSetFactory dbSetFactory)
        {
            _dbSet = dbSetFactory.CreateDbSet<T>();
            _dbSetFactory = dbSetFactory;
        }

        #region IRepository<T> Members

        public IQueryable<T> AsQueryable()
        {
            return _dbSet;
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            return PerformInclusions(includeProperties, query);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> where,
                                    params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.Where(where);
        }

        public T Find(object id)
        {
            return _dbSet.Find(id);
        }

        public T Single(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.SingleOrDefault(where);
        }

        public T First(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.FirstOrDefault(where);
        }

        public void Delete(T entity)
        {
            _dbSetFactory.ChangeObjectState(entity, EntityState.Deleted);
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException();
            _dbSetFactory.ChangeObjectState(entity, EntityState.Modified);
        }

        public T Refresh(T entity)
        {
            _dbSetFactory.RefreshEntity(ref entity);
            return entity;
        }

        #endregion

        private static IQueryable<T> PerformInclusions(IEnumerable<Expression<Func<T, object>>> includeProperties,
                                                        IQueryable<T> query)
        {
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
