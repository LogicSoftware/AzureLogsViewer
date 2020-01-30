using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogAnalyticsViewer.Model.DTO;
using LogAnalyticsViewer.Model.Entities;
using LogAnalyticsViewer.Model.Services.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LogAnalyticsViewer.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueriesController : ControllerBase
    {
        private readonly ILogger<QueriesController> _logger;
        private readonly QueryService _service;

        public QueriesController(
            ILogger<QueriesController> logger,
            QueryService service
        )
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [Route("/api/list")]
        public Task<IEnumerable<Query>> GetList()
        {
            return _service.GetQueries();
        }

        [HttpPost]
        [Route("/api/get")]
        public async Task<Query> GetQuery(int queryId)
        {
            var query = await _service.GetQuery(queryId);
            if (query == null)
            {
                throw new Exception(@$"Query with Id: '{queryId}' not found");
            }

            return query;
        }

        [HttpPost]
        [Route("/api/create")]
        public async Task<int> Create(CreateQueryModel model)
        {
            if (string.IsNullOrEmpty(model.DisplayName))
            {
                throw new Exception("Display Name is required");
            }
            if (string.IsNullOrEmpty(model.QueryText))
            {
                throw new Exception("QueryText is required");
            }
            if (string.IsNullOrEmpty(model.Channel))
            {
                throw new Exception("Channel is required");
            }

            Query newQuery = new Query
            {
                DisplayName = model.DisplayName,
                QueryText = model.QueryText,
                Enabled = model.Enabled,
                Channel = model.Channel
            };

            var query = await _service.CreateQuery(newQuery);
            return query.QueryId;
        }

        [HttpPost]
        [Route("/api/update")]
        public async Task<ActionResult> Update(SaveQueryModel model)
        {
            var query = await _service.GetQuery(model.QueryId);
            if (query == null)
            {
                throw new Exception(@$"Query with Id: '{model.QueryId}' not found");
            }

            if (string.IsNullOrEmpty(model.DisplayName))
            {
                throw new Exception("Display Name is required");
            }
            if (string.IsNullOrEmpty(model.QueryText))
            {
                throw new Exception("QueryText is required");
            }
            if (string.IsNullOrEmpty(model.Channel))
            {
                throw new Exception("Channel is required");
            }

            query.DisplayName = model.DisplayName;
            query.Enabled = model.Enabled;
            query.QueryText = model.QueryText;
            query.Channel = model.Channel;

            await _service.UpdateQuery(query);

            return NoContent();
        }
        
        [HttpPost]
        [Route("/api/delete")]
        public async Task<ActionResult> Delete(int queryId)
        {
            var query = await _service.GetQuery(queryId);

            if (query == null)
            {
                throw new Exception(@$"Query with Id: '{queryId}' not found");
            }

            await _service.DeleteQuery(query);
            return NoContent();
        }

    }
}
