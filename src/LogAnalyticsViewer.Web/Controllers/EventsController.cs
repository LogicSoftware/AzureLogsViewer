using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogAnalyticsViewer.Model;
using LogAnalyticsViewer.Model.DTO;
using LogAnalyticsViewer.Model.Services.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LogAnalyticsViewer.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly EventService _service;
        private readonly LAVDataContext _dbContext;

        public EventsController(
            ILogger<EventsController> logger,
            EventService service,
            LAVDataContext dbContext
        )
        {
            _logger = logger;
            _service = service;
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("/api/search")]
        public Task<List<Event>> Get(EventRequest request)
        {
            var select = _dbContext.Queries.AsQueryable();

            if (request.QueryId != null)
            {
                select = select.Where(q => q.QueryId == request.QueryId);
            }

            var queries = select
                .Select(x => x.QueryText)
                .ToList();

            return _service.GetEventsForClient(queries, request.From, request.To, request.MessageFilters);
        }
    }
}
