using DALayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.Context
{
    public class MenuDbContext : IdentityDbContext<AppUser>
    {
        public MenuDbContext(DbContextOptions options) :base(options) 
        {
           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
          
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(true); 
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<MenuItem> MenuItems { get; set; }  
        public DbSet<Category> Categories { get; set; }  

        public DbSet<AppUser> AppUsers { get; set; }
    }
}
