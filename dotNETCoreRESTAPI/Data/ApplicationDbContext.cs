using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNETCoreRESTAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNETCoreRESTAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
    
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
            
        public DbSet<Product> Products {get; set;}
        public DbSet<Supplier> Suppliers {get; set;}
        public DbSet<Address> Addresses {get; set;}

        
    }
}