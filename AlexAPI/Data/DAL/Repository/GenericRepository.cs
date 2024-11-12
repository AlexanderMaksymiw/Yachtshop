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
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    Expression<Func<TEntity, object>>[] includes = null)
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

        public virtual IEnumerable<T> ExecuteSqlQuery<T>(string sql, params SqlParameter[] parameters) where T : class, new()
        {
            var resultList = new List<T>();

            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                // Add parameters
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }

                context.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    var properties = typeof(T).GetProperties();

                    while (reader.Read())
                    {
                        var entity = new T();
                        foreach (var prop in properties)
                        {
                            var columnName = prop.Name;
                            if (reader.HasRows && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                            {
                                var value = reader[columnName];
                                prop.SetValue(entity, value);
                            }
                        }
                        resultList.Add(entity);
                    }
                }
            }

            return resultList;
        }
    }
}
