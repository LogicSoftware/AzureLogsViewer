using System.Data.Entity;
using System.Transactions;
using AzureLogsViewer.Model.Infrastructure;
using NUnit.Framework;

namespace AzureLogsViewer.Tests
{
    public abstract class BaseIntegrationTest
    {
        private TransactionScope _transactionScope;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AlwDataContext>());

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
        }

        [TearDown]
        public virtual void TearDown()
        {
            //rollback all changes
            _transactionScope.Dispose();
        }
    }
}