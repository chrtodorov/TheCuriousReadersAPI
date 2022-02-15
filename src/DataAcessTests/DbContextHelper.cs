using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccessTests
{
    public static class DbContextHelper
    {
        public static T? CreateInMemoryDatabase<T>() where T : DbContext
        {
            var dbOptions = new DbContextOptionsBuilder<T>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var dataContext = Activator.CreateInstance(typeof(T), dbOptions) as T;
            dataContext?.Database.EnsureCreated();

            return dataContext;
        }
    }
}
