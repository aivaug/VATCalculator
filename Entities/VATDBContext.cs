using Entities.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Entities
{
    public class VATDBContext : DbContext, IDisposable
    {
        public VATDBContext(DbContextOptions<VATDBContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public virtual DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, IsEU = true, Name = "Lithuania", VAT = 21 },
                new Country { Id = 2, IsEU = false, Name = "Russia", VAT = 20 },
                new Country { Id = 3, IsEU = false, Name = "United kingdom", VAT = 20 }
            );

            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, CountryId = 1, IsCompany = true, IsVATPayer = true },
                new Member { Id = 2, CountryId = 2, IsCompany = true, IsVATPayer = true },
                new Member { Id = 3, CountryId = 3, IsCompany = false, IsVATPayer = true },
                new Member { Id = 4, CountryId = 1, IsCompany = false, IsVATPayer = false }
            );
        }
    }
}
