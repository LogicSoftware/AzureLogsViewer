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
    [Route("api/[controller]")]
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
        [Route("list")]
        public Task<IEnumerable<Query>> GetList()
        {
            return _service.GetQueries();
        }

        [HttpPost]
        [Route("get")]
        public async Task<Query> GetQuery([FromBody]IdModel model)
        {
            var query = await _service.GetQuery(model.QueryId);
            if (query == null)
            {
                throw new Exception(@$"Query with Id: '{model.QueryId}' not found");
            }

            return query;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IdModel> Create([FromBody]CreateQueryModel model)
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
            return new IdModel { QueryId = query.QueryId };
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody]SaveQueryModel model)
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
        [Route("delete")]
        public async Task<ActionResult> Delete([FromBody]IdModel model)
        {
            var query = await _service.GetQuery(model.QueryId);

            if (query == null)
            {
                throw new Exception(@$"Query with Id: '{model.QueryId}' not found");
            }

            await _service.DeleteQuery(query);
            return NoContent();
        }

    }
}
