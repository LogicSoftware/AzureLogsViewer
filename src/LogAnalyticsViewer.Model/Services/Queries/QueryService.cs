using LogAnalyticsViewer.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogAnalyticsViewer.Model.Services.Events
{
    public class QueryService
    {
        private LAVDataContext _dbContext { get; set; }

        public QueryService(LAVDataContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<Query>> GetQueries()
        {
            return await _dbContext.Queries.ToListAsync();
        }

        public async Task<Query> GetQuery(int queryId)
        {
            return await _dbContext.Queries.Where(x=>x.QueryId == queryId).SingleOrDefaultAsync();
        }

        public async Task<Query> CreateQuery(Query query)
        {
             _dbContext.Queries.Add(query);
            await _dbContext.SaveChangesAsync();
            return  query;
        }

        public async Task<Query> UpdateQuery(Query query)
        {
             _dbContext.Queries.Update(query);
            await _dbContext.SaveChangesAsync();
            return query;
        }

        public async Task DeleteQuery(Query query)
        {
             _dbContext.Queries.Remove(query);
            await _dbContext.SaveChangesAsync();
        }
    }
}
