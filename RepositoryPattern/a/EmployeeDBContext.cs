using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPattern.a
{
    public class EmployeeDBContext : DbContext
    {
        public virtual DbSet<Employee> Employees { get; set; }

        //public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options)
        //    : base(options)
        //{

        //}

        public EmployeeDBContext() 
        {

        }
    }
}
