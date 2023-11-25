using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> , IGenericRepository
    {
        public DepartmentRepository(MVCSession02DbContext dbContext) : base(dbContext) { }

    }
}
