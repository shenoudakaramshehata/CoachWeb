using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Coach.Data
{
    public class ApplicationUser : IdentityUser
    {

        public string FullName { get; set; }

        public int CountryId { get; set; }
        public string Pic { get; set; }

    }
    public class ApplicationDbContext : IdentityDbContext
    {
    
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.FullName);
            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.CountryId);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Name = "Admin", NormalizedName = "Admin".ToUpper() });
           

        }
    }
}
