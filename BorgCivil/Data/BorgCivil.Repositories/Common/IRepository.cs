using BorgCivil.Framework.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace BorgCivil.Repositories
{
    public interface IRepository
    {
        AppIdentityDbContext GetContext();
        T GetById<T>(Guid id) where T : class;
        IQueryable<T> GetAll<T>() where T : class;
        T FindBy<T>(object parameter) where T : class;
        T FindBy<T>(Expression<Func<T, bool>> filter) where T : class;
        T FindBy<T>(Expression<Func<T, bool>> filter, string include) where T : class;
        IQueryable<T> SearchBy<T>(Expression<Func<T, bool>> filter) where T : class;
        IQueryable<T> SearchBy<T>(Expression<Func<T, bool>> filter, string include) where T : class;
        IEnumerable<T> SqlQuery<T>(string sql) where T : class;
        IEnumerable<T> SqlQuery<T>(string sql, object[] parameters) where T : class;
        bool Exists<T>(T entity) where T : class;
        bool Exists<T>(params object[] keys) where T : class;
        bool Exists<T>(Expression<Func<T, bool>> filter) where T : class;
        IEnumerable<System.Web.Mvc.SelectListItem> GetSelectList<T>(string text, string value, string selected) where T : class;
        void Insert<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Update<T>(List<T> entities) where T : class;
        void Delete<T>(T entity) where T : class;
        void Delete<T>(List<T> entities) where T : class;
        void Commit();
        void Dispose();
    }
}
