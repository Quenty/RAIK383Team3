﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PracticalWerewolf.Stores.Interfaces
{
    public interface IEntityStore<T> where T : class
    {
        IQueryable<T> AsQueryable();
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> Find(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        T Find(object id);
        T Single(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        T First(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includeProperties);
        void Insert(T entity);
    }
}