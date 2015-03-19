using System;
using System.Data.Entity;
using System.Transactions;
using AzureLogsViewer.Model.Infrastructure;
using AzureLogsViewer.Model.Services;
using NUnit.Framework;

namespace AzureLogsViewer.Tests
{
    public abstract class BaseIntegrationTest
    {
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

            WadLogsService.UtcNowTestsOverride = null;
            ResetDataContext();
        }

        [TearDown]
        public virtual void TearDown()
        {
            //rollback all changes
            _transactionScope.Dispose();
        }

        protected void ResetDataContext()
        {
            if(DataContext != null)
                DataContext.Dispose();

            DataContext = new AlwDataContext();
        }
    }
}