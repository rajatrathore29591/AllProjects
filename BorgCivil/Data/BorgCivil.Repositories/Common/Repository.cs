
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using BorgCivil.Framework.Identity;
using BorgCivil.Repositories;

namespace BorgCivil.Repositories
{
    public class Repository : IRepository
    {
        protected AppIdentityDbContext context = ContextFactory.GetContext();

        public Repository()
        {
            this.context.Configuration.ProxyCreationEnabled = true;
            this.context.Configuration.LazyLoadingEnabled = true;
        }

        #region CREATE METHOD

        /// <summary>
        /// This method for insert record is database.
        /// </summary>
        /// <typeparam name="T">object entity</typeparam>
        /// <param name="entity">entity</param>
        public void Insert<T>(T entity) where T : class
        {
            try
            {
                DbSet<T> _set = context.Set<T>();

                _set.Add(entity);
                //Commit();
                //context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        #endregion

        #region RETRIVE METHOD

        ////Get Entity

        /// <summary>
        /// This method using for get single record from database entity by id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">ID (Primary Key)</param>
        /// <returns>Entity Object</returns>
        public T GetById<T>(Guid id) where T : class
        {
            try
            {
                return context.Set<T>().Find(id);
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

        }

        /// <summary>
        /// This method using for get all record from database entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>All Entity Object</returns>
        public IQueryable<T> GetAll<T>() where T : class
        {
            try
            {
                return context.Set<T>();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        ////Single Object Type Method

        /// <summary>
        /// This method using for get single record from database entity by key parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter">key value parameter</param>
        /// <returns>Single Entity Object</returns>
        public T FindBy<T>(object parameter) where T : class
        {
            try
            {
                return context.Set<T>().Find(parameter);
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        /// <summary>
        ///This method using for get single record from database entity by key filter expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">condition</param>
        /// <returns>Single Entity Object</returns>
        public T FindBy<T>(Expression<Func<T, bool>> filter) where T : class
        {
            try
            {
                return context.Set<T>().Where(filter).FirstOrDefault();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        /// <summary>
        ///  This method using for get single record from database entity by key filter expression including additional.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">condition</param>
        /// <param name="include">include</param>
        /// <returns>Single Entity Object</returns>
        public T FindBy<T>(Expression<Func<T, bool>> filter, string include) where T : class
        {
            try
            {
                return context.Set<T>().Where(filter).Include(include).FirstOrDefault();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        ////Collection Type Methods

        /// <summary>
        /// This method using for get filter records from database entity by key filter expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns>List</returns>
        public IQueryable<T> SearchBy<T>(Expression<Func<T, bool>> filter) where T : class
        {
            try
            {
                return context.Set<T>().Where(filter).AsQueryable();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }


        }


        /// <summary>
        /// This method using for get filter records from database entity by key filter expression including additional.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">expression</param>
        /// <param name="include">include</param>
        /// <returns>List</returns>
        public IQueryable<T> SearchBy<T>(Expression<Func<T, bool>> filter, string include) where T : class
        {
            try
            {
                return context.Set<T>().Where(filter).Include(include);
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }


        /// <summary>
        /// This method using for get Select List from database Key Value pair.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Select List Itams for List Type Controls</returns>
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectList<T>(string text, string value, string selected) where T : class
        {
            try
            {
                IEnumerable<T> result = context.Set<T>();
                var query = from e in result
                            select new
                            {
                                Value = e.GetType().GetProperty(value).GetValue(e, null),
                                Text = e.GetType().GetProperty(text).GetValue(e, null)
                            };

                return query.AsEnumerable()
                    .Select(s => new System.Web.Mvc.SelectListItem
                    {
                        Value = s.Value.ToString(),
                        Text = s.Text.ToString(),
                        Selected = (selected == s.Value.ToString() ? true : false)
                    });
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        #endregion

        #region UPDATE METHOD

        /// <summary>
        /// This method for update single record is database.
        /// </summary>
        /// <typeparam name="T">object entity</typeparam>
        /// <param name="entity">entity</param>
        public void Update<T>(T entity) where T : class
        {
            try
            {
                DbSet<T> _set = context.Set<T>();
                _set.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
                //context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch(Exception ex) { throw; }
        }

        /// <summary>
        ///  This method for update bulk record is database.
        /// </summary>
        /// <typeparam name="T">object entity</typeparam>
        /// <param name="entities">entity</param>
        public void Update<T>(List<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        #endregion

        #region  DELETE METHOD
        /// <summary>
        ///  This method for delete single record is database.
        /// </summary>
        /// <typeparam name="T">object entity</typeparam>
        /// <param name="entity">entity</param>
        public void Delete<T>(T entity) where T : class
        {
            try
            {
                DbSet<T> _set = context.Set<T>();
                var entry = context.Entry(entity);
                if (entry != null)
                {
                    entry.State = EntityState.Deleted;
                }
                else
                {
                    _set.Attach(entity);
                }
                context.Entry(entity).State = EntityState.Deleted;
                //context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        /// <summary>
        ///  This method for delete bulk record is database.
        /// </summary>
        /// <typeparam name="T">object entity</typeparam>
        /// <param name="entities">entity</param>
        public void Delete<T>(List<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        #endregion

        //// Collection Type Methods SQL Query

        /// <summary>
        /// This method using for get records from database by sql query expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns>entity</returns>
        public IEnumerable<T> SqlQuery<T>(string sql) where T : class
        {
            try
            {
                return context.Set<T>().SqlQuery(sql);
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

        }

        /// <summary>
        /// This method using for get records from database by sql query with filter expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">Sql Query</param>
        /// <param name="parameters">Filter Parameters</param>
        /// <returns>Set of Entity</returns>
        public IEnumerable<T> SqlQuery<T>(string sql, object[] parameters) where T : class
        {
            try
            {
                return context.Set<T>().SqlQuery(sql, parameters);
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        ////Result Type Method

        /// <summary>
        /// This method using for checked record is exist in database entity by entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Exists<T>(T entity) where T : class
        {
            return context.Set<T>().Local.Any(e => e == entity);
        }

        /// <summary>
        /// This method using for checked record is exist in database entity by key object arry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool Exists<T>(params object[] keys) where T : class
        {
            return (context.Set<T>().Find(keys) != null);
        }

        /// <summary>
        /// This method using for checked record is exist in database entity by key filter expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter">expression</param>
        /// <returns>true/false</returns>
        public bool Exists<T>(Expression<Func<T, bool>> filter) where T : class
        {
            try
            {
                return context.Set<T>().Where(filter).FirstOrDefault() != null ? true : false;
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The exception errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        /// <summary>
        /// Get context for entities.
        /// </summary>
        /// <returns>IdentityDbContext Object</returns>
        public AppIdentityDbContext GetContext()
        {
            return context;
        }

        /// <summary>
        /// Save record from database.
        /// </summary>
        public void Commit()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Exception raise = ex;
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        /// <summary>
        /// Dispose Context
        /// </summary>
        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
                context = null;
            }
        }

    }
}
