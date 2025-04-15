using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using NUnit.Framework;

namespace TestingLayer
{
    [TestFixture]
    public class TestManager
    {
        internal static GameDbContext dbContext;
        static TestManager()
        {
            DbContextOptionsBuilder<GameDbContext> builder = new DbContextOptionsBuilder<GameDbContext>();
            builder.UseInMemoryDatabase("TestDb");
            dbContext = new GameDbContext(builder.Options);

        }
        
        [SetUp]
        public void RandomSetup()
        {
            DbContextOptionsBuilder<GameDbContext> builder = new DbContextOptionsBuilder<GameDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString()); 
            dbContext = new GameDbContext(builder.Options);
        }

        [TearDown]
        public void RandomDispose()
        {
            dbContext.Dispose();
        }

        
    }
}
