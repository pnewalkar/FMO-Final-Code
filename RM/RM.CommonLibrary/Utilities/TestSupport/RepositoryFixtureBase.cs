﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;

namespace RM.CommonLibrary.HelperMiddleware
{
    public abstract class RepositoryFixtureBase : TestFixtureBase
    {
        public static Mock<DbSet<T>> MockDbSet<T>(IEnumerable<T> list)
            where T : class, new()
        {
            IQueryable<T> queryableList = list.AsQueryable();
            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryableList.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryableList.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryableList.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(queryableList.GetEnumerator());
            dbSetMock.Setup(x => x.Create()).Returns(new T());

            return dbSetMock;
        }
    }
}