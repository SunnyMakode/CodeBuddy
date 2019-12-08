using CodeBuddy.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeBuddy.Api.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> contextOptions) 
            : base(contextOptions) { }

        public DbSet<Outcome> Outcomes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
