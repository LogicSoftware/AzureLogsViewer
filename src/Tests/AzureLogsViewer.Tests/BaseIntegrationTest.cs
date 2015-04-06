using System;
using System.Data.Entity;
using System.Transactions;
using AzureLogsViewer.Model;
using AzureLogsViewer.Model.Infrastructure;
using AzureLogsViewer.Model.Services;
using Ninject;
using Ninject.Extensions.ChildKernel;
using NUnit.Framework;

namespace AzureLogsViewer.Tests
{
    public abstract class BaseIntegrationTest
    {
        private static Lazy<IKernel> _mainKernel = new Lazy<IKernel>(CreateMainKernel);
        private IKernel _kernel;

        private TransactionScope _transactionScope;

        public AlwDataContext DataContext { get; private set; }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Database.SetInitializer(new TestDatabaseInitializer());

            //create database
            using (var context = new AlwDataContext())
            {
                context.Database.Initialize(false);
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            var transactionOptions = new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            };

            _transactionScope = new TransactionScope(TransactionScopeOption.Required, transactionOptions);

            //we can make some overrides in di configuration in tests which won't affect each other
            _kernel = new ChildKernel(_mainKernel.Value);

            WadLogsDumpService.UtcNowTestsOverride = null;
            ResetDataContext();
        }

        [TearDown]
        public virtual void TearDown()
        {
            //rollback all changes
            _transactionScope.Dispose();
            _kernel.Dispose();
        }

        protected T GetService<T>()
        {
            return _kernel.Get<T>();
        }

        protected void ResetDataContext()
        {
            if(DataContext != null)
                DataContext.Dispose();

            DataContext = _kernel.Get<AlwDataContext>();
        }

        private static IKernel CreateMainKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(new DIModule());

            return kernel;
        }
    }
}