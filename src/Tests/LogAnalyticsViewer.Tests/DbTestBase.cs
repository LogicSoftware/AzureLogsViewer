using System.Linq;
using LogAnalyticsViewer.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogAnalytics.Tests;

public class DbTestBase
{
    private SqliteConnection _connection = null!;
    private DbContextOptions<LAVDataContext> _contextOptions = null!;

    [TestInitialize]
    public virtual void TestInitialize()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        _contextOptions = new DbContextOptionsBuilder<LAVDataContext>()
            .UseSqlite(_connection)
            .Options;
        
        using var context = new LAVDataContext(_contextOptions);
        context.Database.EnsureCreated();
    }

    protected LAVDataContext CreateContext(bool empty=false)
    {
        var context = new LAVDataContext(_contextOptions);
        if (empty)
        {
            context.Queries.RemoveRange(context.Queries.ToList());
            context.SaveChanges();
        }
        return context;
    }

    [TestCleanup]
    public virtual void TestCleanup()
    {
        _connection.Dispose();
    }
}
