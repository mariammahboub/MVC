using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfwork
    {
        private readonly MVCSession02DbContext _dbContext;

        public EmployeeRepository EmployeeRepository { get ; set ; }
        public DepartmentRepository DepartmentRepository { get ; set ; }

        public UnitOfWork(MVCSession02DbContext dbContext)
        {
            _dbContext = dbContext;
            EmployeeRepository = new EmployeeRepository(dbContext);
            DepartmentRepository = new DepartmentRepository(dbContext);
        }
        public async Task<int> Complete()
        { 
            return await  _dbContext.SaveChangesAsync(); 
        }

        public async ValueTask DisposeAsync()
            =>  await  _dbContext.DisposeAsync();

        
    }
}
