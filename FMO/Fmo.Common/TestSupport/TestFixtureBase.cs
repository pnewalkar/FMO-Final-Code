using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Fmo.Common.TestSupport
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
        /// <summary>
        /// Gets the mock factory.
        /// </summary>
        /// <value>The mock factory.</value>
        /// <remarks>
        /// Attention: Creating mocks directly with the this mock
        /// factory instance will not register them in the <c>CreatedMockObjects</c>
        /// collection.
        /// </remarks>
        protected MockRepository TestMockRepository
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the created view models.
        /// </summary>
        /// <value>The created view models.</value>
        protected List<object> CreatedMockObjects
        {
            get;
            private set;
        }

        /// <summary>
        /// Setups the per-test instances.
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            CreatedMockObjects = new List<object>();
            TestMockRepository = new MockRepository(MockBehavior.Loose);

            OnSetup();
        }

        /// <summary>
        /// Configures if the test should use the unit of work mocks. In some test cases (e.g. integration tests),
        /// this method should be overriden to return false.
        /// </summary>
        /// <returns>Default implementation returns true.</returns>
        protected virtual bool ShouldUseUnitOfWorkAndTransactionMocks()
        {
            return true;
        }

        /// <summary>
        /// Creates a standard mock object for the given type.
        /// </summary>
        /// <typeparam name="T">The type of the object that shall be mocked.</typeparam>
        /// <returns>The created mock.</returns>
        /// <remarks>The created mock is registered in the <c>CreatedMockObjects</c> collection.</remarks>
        protected Mock<T> CreateMock<T>()
            where T : class
        {
            Mock<T> mock = TestMockRepository.Create<T>();
            CreatedMockObjects.Add(mock.Object);
            return mock;
        }

        /// <summary>
        /// Gets a list of the created mocks by type.
        /// </summary>
        /// <typeparam name="T">The type of the object which mocks should be resolved.</typeparam>
        /// <returns>A list of the created mocks for the specified type.</returns>
        protected List<Mock<T>> GetCreatedMocksByType<T>()
            where T : class
        {
            return CreatedMockObjects.FindAll(vm => vm is T).
                    ConvertAll<Mock<T>>(vm => Mock.Get<T>((T)vm));
        }

        /// <summary>
        /// Gets the first instance of the type of the created object. If there is more than one instance of this type,
        /// </summary>
        /// <typeparam name="T">The type of the object which mocks should be resolved.</typeparam>
        /// <returns>A list of the created view model mocks for the specified type.</returns>
        protected Mock<T> GetMockByType<T>()
            where T : class
        {
            var allCreatedMocks = CreatedMockObjects.FindAll(vm => vm is T).
                    ConvertAll<Mock<T>>(vm => Mock.Get<T>((T)vm));

            if (allCreatedMocks.Count != 1)
            {
                string msg = string.Format(
                                       CultureInfo.InvariantCulture,
                                       "Unexpected state of the inner mock collection. Expected exactly one mock of type {0} but found {1}.",
                                       typeof(T),
                                       allCreatedMocks.Count);
                throw new InvalidOperationException(msg);
            }

            return allCreatedMocks.First();
        }

        /// <summary>
        /// Gets a list of the created mocks by type.
        /// </summary>
        /// <typeparam name="T">The type of the object which mocks should be resolved.</typeparam>
        /// <returns>A list of the created mocks for the specified type.</returns>
        protected List<T> GetCreatedMockObjectsByType<T>()
            where T : class
        {
            return CreatedMockObjects.FindAll(vm => vm is T).
                    ConvertAll<T>(vm => (T)vm);
        }

        /// <summary>
        /// Called before each test runs.
        /// Finally, it creates a MockFactory with MockBehavior.Loose.
        /// </summary>
        protected abstract void OnSetup();
    }
}