using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TestDemo.Data.Entities;

namespace TestDemo.Data
{
  public  class AppDataContext : DbContext
    {
        public AppDataContext() { }

        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=CTPLBEDTP056\SQLEXPRESS;Initial Catalog=TestDemo;Persist Security Info=True;User ID=sa;Password=Cignex@123");
        }

        public DbSet<Employee> Employee { get; set; }
    }
}
