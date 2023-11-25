using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly MVCSession02DbContext _dbContext;
        public GenericRepository(MVCSession02DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(T entity)
            => _dbContext.Add(entity);
        

        public void Delete(T entity)
            => _dbContext.Remove(entity);
        

        public async Task<T> Get(int id)
            => await _dbContext.Set<T>().FindAsync(id);

        public  async Task<IEnumerable<T>> GetAll()
        {
            if(typeof(T) == typeof(Employee))
                return (IEnumerable<T>)  await _dbContext.Employees.Include(E => E.Department).AsNoTracking().ToListAsync();
            else
                return  await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }
            

        public void Update(T entity)
        => _dbContext.Update(entity);
        
    }
}
