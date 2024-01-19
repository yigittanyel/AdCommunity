using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Services.Reporting;
using AdCommunity.Application.Services.Reporting.Consts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> EventListReport(CancellationToken cancellationToken)
        {
            var stream = await _reportService.EventListReportAsync(cancellationToken);
            byte[] bytes = stream.ToArray();
            return File(bytes, ReportingConsts.ContentType, ReportingConsts.EventListReportName);
        }
    }
}
