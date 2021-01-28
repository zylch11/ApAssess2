using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApAssess2.Models;
using Microsoft.EntityFrameworkCore;

namespace ApAssess2.Data {
    
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {  }

        public DbSet<Category> Category { get; set; }

        public DbSet<Product> Product { get; set; }
    }

}