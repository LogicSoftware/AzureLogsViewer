using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public EventsController(
            ILogger<EventsController> logger,
            EventService service
        )
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [Route("/api/search")]
        public Task<IEnumerable<Event>> Get(EventRequest request)
        {
            return _service.GetEvents(request.From, request.To, /*TODO*/ null, request.MessageFilters);
        }
    }
}
