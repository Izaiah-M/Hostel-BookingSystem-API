using HostME.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HostME.Core.UnitOfWork.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly HostMeContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(HostMeContext context)
        {
            _context = context;
            _db = context.Set<T>();
        }

        // Getting one item
        public async Task<T?> Get(Expression<Func<T, bool>> expression, List<string>? includes = null)
        {
            IQueryable<T> query = _db;

            if (includes is not null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        // Getting all
        public async Task<IList<T>> GetAll(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<string>? includes = null)
        {
            IQueryable<T> query = _db;

            // If someone passes a lambda expression. Make a query in the DB and find what makes that lambda expression true
            if (expression != null)
            {

                query = query.Where(expression);
            }


            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        // Inserting an Item
        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity);
        }

        // Inserting many at once
        public async Task InsertRange(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);
        }

        // Updating and entity
        public void Update(T entity)
        {
            _db.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;
        }

        // Deleting an entity
        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);

            if (entity != null)
            {
                _db.Remove(entity);
            }
        }

        // Deleting many entities
        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }
    }
}
