using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace AlexAPI.Data.DAL.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Expression<Func<TEntity, object>>[]? includes = null
        )
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public IEnumerable<T> ExecuteSqlQuery<T>(string sql, object parameters = null) where T : class
        {
            using var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection.Query<T>(sql, parameters);
        }

        public IEnumerable<T1> ExecuteMultiMapQuery<T1, T2>(
            string sql,
            Func<T1, T2, T1> map,
            string splitOn,
            object parameters = null)
        where T1 : class
        {
            using var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection.Query(sql, map, parameters, splitOn: splitOn);
        }

        public IEnumerable<T1> ExecuteMultiMapQuery<T1, T2, T3>(
            string sql,
            Func<T1, T2, T3, T1> map,
            string splitOn,
            object parameters = null)
        where T1 : class
        {
            using var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection.Query(sql, map, parameters, splitOn: splitOn);
        }

        public IEnumerable<T1> ExecuteMultiMapQuery<T1, T2, T3, T4>(
            string sql,
            Func<T1, T2, T3, T4, T1> map,
            string splitOn,
            object parameters = null)
        where T1 : class
        {
            using var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection.Query(sql, map, parameters, splitOn: splitOn);
        }

        public IEnumerable<T1> ExecuteMultiMapQuery<T1, T2, T3, T4, T5>(
            string sql,
            Func<T1, T2, T3, T4, T5, T1> map,
            string splitOn,
            object parameters = null)
        where T1 : class
        {
            using var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection.Query(sql, map, parameters, splitOn: splitOn);
        }

        public IEnumerable<T1> ExecuteMultiMapQuery<T1, T2, T3, T4, T5, T6>(
            string sql,
            Func<T1, T2, T3, T4, T5, T6, T1> map,
            string splitOn,
            object parameters = null)
        where T1 : class
        {
            using var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection.Query(sql, map, parameters, splitOn: splitOn);
        }

        public IEnumerable<T1> ExecuteMultiMapQuery<T1, T2, T3, T4, T5, T6, T7>(
            string sql,
            Func<T1, T2, T3, T4, T5, T6, T7, T1> map,
            string splitOn,
            object parameters = null)
        where T1 : class
        {
            using var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                connection.Open();

            return connection.Query(sql, map, parameters, splitOn: splitOn);
        }
    }
}
