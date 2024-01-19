using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.DTOs.Event;

namespace AdCommunity.Application.Services.Reporting;
public interface IReportService
{
    Task<MemoryStream> EventListReportAsync(CancellationToken cancellationToken);
}
