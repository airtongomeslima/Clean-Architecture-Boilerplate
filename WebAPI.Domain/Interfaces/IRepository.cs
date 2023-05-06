using WebAPI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebAPI.Domain.Interface
{
    public interface IRepository<T> where T : Entity
    {
        int Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        T FindById(int id);
        T[] FindAll();
        T[] FindBy(Expression<Func<T, bool>> predicate);
        int Count();
        int Create(IEnumerable<T> entities);
        int DeleteBy(Expression<Func<T, bool>> predicate);
        int Update(IEnumerable<T> entities);
        bool CreateOrUpdate(T entity);
        int CreateOrUpdate(IEnumerable<T> entities);
        bool DropCollection(string collectionName);
        void CreateTable();
    }
}
