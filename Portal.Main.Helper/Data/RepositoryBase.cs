// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="Sliding Stones">
//   Copyright © 2015 All Right Reserved
// </copyright>
// <summary>
//   http://www.dotnetcurry.com/entityframework/1170/transaction-scope-databases-adonet-entity-framework-aspnet-mvc
//   http://stackoverflow.com/questions/26676563/entity-framework-queryable-async
//   https://github.com/tugberkugurlu/GenericRepository
//   http://codereview.stackexchange.com/questions/19037/entity-framework-generic-repository-pattern
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Portal.Main.Helper.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// http://www.dotnetcurry.com/entityframework/1170/transaction-scope-databases-adonet-entity-framework-aspnet-mvc
    ///     http://stackoverflow.com/questions/26676563/entity-framework-queryable-async
    ///     https://github.com/tugberkugurlu/GenericRepository
    ///     http://codereview.stackexchange.com/questions/19037/entity-framework-generic-repository-pattern
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class RepositoryBase<T> : IRepository<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{T}"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        protected RepositoryBase(DbContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<T>();
        }

        /// <summary>
        ///     Gets the context.
        /// </summary>
        public DbContext Context { get; }

        /// <summary>
        ///     Gets the db set.
        /// </summary>
        public DbSet<T> DbSet { get; }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public virtual T GetById(object id)
        {
            return this.DbSet.Find(id);
        }

        /// <summary>
        /// The get by id async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Task<T> GetByIdAsync(object id)
        {
            return this.DbSet.FindAsync(id);
        }

        /// <summary>
        ///     The get all.
        /// </summary>
        /// <returns>
        ///     The <see cref="IQueryable" />.
        /// </returns>
        public virtual IQueryable<T> GetAll()
        {
            return this.DbSet.Select(e => e);
        }

        /// <summary>
        ///     The get all async.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public virtual Task<List<T>> GetAllAsync()
        {
            return this.DbSet.Select(e => e).ToListAsync();
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="cacheTime">
        /// The cache time.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public virtual IEnumerable<T> GetAll(TimeSpan cacheTime)
        {
            return this.DbSet.Select(e => e); // .FromCache(CachePolicy.WithDurationExpiration(cacheTime));
        }

        /// <summary>
        /// The exists.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool Exists(T entity)
        {
            return this.DbSet.Any(e => e == entity);
        }

        /// <summary>
        /// The exists async.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public virtual async Task<bool> ExistsAsync(T entity)
        {
            return await this.DbSet.AnyAsync(e => e == entity);
        }

        /// <summary>
        /// The exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool Exists(object id)
        {
            return this.DbSet.Find(id) != null;
        }

        /// <summary>
        /// The exists async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public virtual async Task<bool> ExistsAsync(object id)
        {
            return await this.DbSet.FindAsync(id) != null;
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public virtual T Add(T entity)
        {
            var addedEntity = this.DbSet.Add(entity);

            if (this.Context.Entry(addedEntity).State == EntityState.Added)
            {
                return addedEntity;
            }

            return null;
        }

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public virtual IEnumerable<T> Add(IEnumerable<T> entities)
        {
            var added = this.DbSet.AddRange(entities);
            return added;
        }

        /// <summary>
        /// The add async.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public virtual async Task<int> AddAsync(T t)
        {
            this.Context.Set<T>().Add(t);
            return await this.Context.SaveChangesAsync();
        }

        /// <summary>
        ///     The count.
        /// </summary>
        /// <returns>
        ///     The <see cref="long" />.
        /// </returns>
        public virtual long Count()
        {
            return this.DbSet.Count();
        }

        /// <summary>
        ///     The count async.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public virtual async Task<int> CountAsync()
        {
            return await this.Context.Set<T>().CountAsync();
        }

        /// <summary>
        /// The count.
        /// </summary>
        /// <param name="whereExpression">
        /// The where expression.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        public virtual long Count(Expression<Func<T, bool>> whereExpression)
        {
            return this.DbSet.Count(whereExpression);
        }

        /// <summary>
        /// The count async.
        /// </summary>
        /// <param name="whereExpression">
        /// The where expression.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await this.Context.Set<T>().CountAsync(whereExpression);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool Delete(T entity)
        {
            this.Context.Set<T>().Attach(entity);
            var deletedEntity = this.DbSet.Remove(entity);
            return this.Context.Entry(deletedEntity).State == EntityState.Deleted;
        }

        /// <summary>
        /// The delete by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool DeleteById(object id)
        {
            var entity = this.GetById(id);
            return this.Delete(entity);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="entities">
        /// The entities.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool Delete(IEnumerable<T> entities)
        {
            this.DbSet.RemoveRange(entities);
            return this.Save();
        }

        /// <summary>
        /// The delete async.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public virtual async Task<bool> DeleteAsync(T t)
        {
            this.Context.Entry(t).State = EntityState.Deleted;
            return await this.SaveAsync();
        }

        /// <summary>
        ///     The save.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public virtual bool Save()
        {
            if (this.Context.IsDirty())
            {
                return this.Context.SaveChanges() > 0;
            }

            return true;
        }

        /// <summary>
        ///     The save async.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        public virtual async Task<bool> SaveAsync()
        {
            if (this.Context.IsDirty())
            {
                return await this.Context.SaveChangesAsync() > 0;
            }

            return true;
        }

        /// <summary>
        /// The get by query.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public virtual IQueryable<T> GetByQuery(Expression<Func<T, bool>> filter)
        {
            return this.DbSet.Where(filter);
        }

        /// <summary>
        /// The get by query.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="cacheTime">
        /// The cache time.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public virtual IEnumerable<T> GetByQuery(Expression<Func<T, bool>> filter, TimeSpan cacheTime)
        {
            return this.DbSet.Where(filter);

            // .FromCache(CachePolicy.WithDurationExpiration(cacheTime));
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="t">
        /// The t.
        /// </param>
        public virtual void Update(T t)
        {
            DbEntityEntry dbEntityEntry = this.Context.Entry(t);

            this.Context.Set<T>().Attach(t);
            dbEntityEntry.State = EntityState.Modified;
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="t">
        /// The t.
        /// </param>
        public virtual void Update(object id, T t)
        {
            var obj = this.GetById(id);
            DbEntityEntry dbEntityEntry = this.Context.Entry(t);
            this.Context.Entry(obj).CurrentValues.SetValues(t);
        }

        // {

        // if (whereExpression != null)

        // var result = this.DbSet.AsQueryable();
        // var skip = (pageNum - 1) * pageSize;
        // {
        // out int totalCount)
        // int pageSize,
        // int pageNum,
        // bool isDescending,
        // Expression<Func<T, TOrderBy>> orderBy,
        // Expression<Func<T, bool>> whereExpression,

        // public virtual IQueryable<T> GetPaged<TOrderBy>(

        // }
        // }
        // this.DbSet = null;
        // this.Context = null;
        // this.Context.Dispose();
        // {
        // if (disposing)
        // {
        // protected virtual void Dispose(bool disposing)

        // The bulk of the clean-up code is implemented in Dispose(bool)
        // }
        // GC.SuppressFinalize(this);
        // this.Dispose(true);                        
        // {

        // public void Dispose()
        // result = result.Where(whereExpression);
        // }

        // if (orderBy != null)
        // {
        // result = isDescending ? result.OrderByDescending(orderBy) : result.OrderBy(orderBy);
        // }
        // else
        // {
        // throw new Exception("To do Paging you MUST provide valid OrderBy value");
        // }

        // result = result.Skip(skip).Take(pageSize);

        // totalCount = whereExpression != null ? this.DbSet.Count(whereExpression) : this.DbSet.Count();

        // return result;
        // }

        // public virtual async Task<List<T>> GetPagedAsync<TOrderBy>(
        // Expression<Func<T, bool>> whereExpression,
        // Expression<Func<T, TOrderBy>> orderBy,
        // bool isDescending,

        // int pageNum,
        // int pageSize)
        // {
        // var skip = (pageNum - 1) * pageSize;

        // var result = this.DbSet.AsQueryable();

        // if (whereExpression != null)
        // {
        // result = result.Where(whereExpression);
        // }

        // if (orderBy != null)
        // {
        // result = isDescending ? result.OrderByDescending(orderBy) : result.OrderBy(orderBy);
        // }
        // else
        // {
        // throw new Exception("To do Paging you MUST provide valid OrderBy value");
        // }

        // result = result.Skip(skip).Take(pageSize);

        // return await result.ToListAsync();
        // }
    }
}