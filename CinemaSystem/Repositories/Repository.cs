//using CinemaSystem.Areas.Admin.Data;
using CinemaSystem.DataAcess;
using CinemaSystem.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
//using CinemaSystem.Areas.Customer.Data;
namespace CinemaSystem.Repository
{
    public class Repository<T>  : IRepository<T> where T : class 
    {
       ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public void DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public async Task<int> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }

        }
        public async Task <IEnumerable<T>> GetAsync(Expression <Func<T,bool>>? expression = null, Expression<Func<T, object>>[] ? includes = null, bool tracked = true)
        {
            //var entities = _dbSet.AsQueryable();
            IQueryable<T> entities = _dbSet;
            if (expression is not null)
            {
                entities = entities.Where(expression);
                
            }
            if (includes != null)
            {
               foreach(var entity in includes)
                {
                    entities = entities.Include(entity);

                }
            }
            if (!tracked)
                entities = entities.AsNoTracking();
            return await entities.ToListAsync();
        }
        public async Task<T>? GetOneAsync(Expression<Func<T, bool>>? expression=null, Expression<Func<T, object>>[]? includes=null, bool tracked = true)
        {
            var list = await GetAsync(expression, includes, tracked);
            //return (await GetAsync(expression , includes ,tracked).FirstOrDefault());
            return list.FirstOrDefault();
        }
    }
    }
