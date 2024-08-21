using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.Excel;
using WebApplication1.Models.Stored_Proc;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
		protected readonly IConfiguration Configuration;

		public ApplicationDbContext(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
		}
		//public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  //      {
            
  //      }
        public DbSet<Category> Categories {  get; set; } // the name given here will represent the db table name
        

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Category>().HasData(
        //        new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
        //        new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
        //        new Category { Id = 3, Name = "History", DisplayOrder = 3 }
        //        );
        //}

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
}
