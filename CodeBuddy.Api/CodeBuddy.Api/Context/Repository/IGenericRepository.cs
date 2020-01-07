﻿using CodeBuddy.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodeBuddy.Api.Context.Repository
{
    public interface IGenericRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<T>> GetAll<T>(Expression<Func<T, object>> includes) where T : class;
        Task<T> Get<T>(int id
            , Expression<Func<T, object>> includes
            , Expression<Func<T, bool>> predicate) where T : class;
    }
}
