using System;
using System.Linq;
using Moq;

namespace AppleStore.Tests.Infrastructure
{
    public static class TestHelpers
    {
        private static readonly Random random = new Random();

        public static Mock<System.Data.Entity.DbSet<T>> GetDbSetMock<T>(IQueryable<T> items = null) where T : class
        {
            var dbSetMock = new Mock<System.Data.Entity.DbSet<T>>();
            var q = dbSetMock.As<IQueryable<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(items.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(items.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(items.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
                .Returns(items.GetEnumerator());

            dbSetMock.Setup(s => s.Add(It.IsAny<T>())).Callback<T>((items.ToList()).Add);


            q.Setup(x => x.GetEnumerator()).Returns(items.GetEnumerator());

            return dbSetMock;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
