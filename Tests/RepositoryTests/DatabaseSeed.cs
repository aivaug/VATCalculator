using Entities;
using Entities.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.RepositoryTests
{
    public class DatabaseSeed : IDisposable
    {
        protected VATDBContext Context
        {
            get; private set;
        }
        public DatabaseSeed()
        {
            var options = new DbContextOptionsBuilder<VATDBContext>()
                            .UseInMemoryDatabase("fakeDb")
                            .Options;

            Context = new VATDBContext(options);
        }

        protected async void AddSampleData()
        {
            if (await Context.Members.AnyAsync()) return;

            var country = new Country { Id = 1 };
            var member = new Member
            {
                Id = 1,
                CountryId = 1,
                IsCompany = false,
                IsVATPayer = true,
                Country = country
            };
            var memberDeleted = new Member
            {
                Id = 2,
                CountryId = 1,
                IsCompany = true,
                IsVATPayer = true,
                Country = country,
                IsDeleted = true
            };
            var memberCompany = new Member
            {
                Id = 3,
                CountryId = 1,
                IsCompany = true,
                IsVATPayer = true,
                Country = country
            };
            Context.Members.AddRange(member, memberDeleted, memberCompany);
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        [TestCleanup]
        public void TestClean()
        {
            Context.Database.EnsureDeleted();
        }
    }
}
