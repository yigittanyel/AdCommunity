using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Features.Event.Queries.GetEventsQuery;
using AdCommunity.Application.Services.Reporting.Consts;
using AdCommunity.Core.CustomMediator.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace AdCommunity.Application.Services.Reporting;
public class ReportService : IReportService
{
    private readonly IYtMediator _mediator;
    public ReportService(IYtMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<MemoryStream> EventListReportAsync(CancellationToken cancellationToken)
    {
        //EpPlus license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //events data fetching
        GetEventsQuery query = new GetEventsQuery();
        IEnumerable<EventDto> events = await _mediator.Send(query, cancellationToken);

        // define excel headers and rows
        var excelHeaders = new List<string>
        {
            ReportingConsts.EventListReportSheetHeader1,
            ReportingConsts.EventListReportSheetHeader2,
            ReportingConsts.EventListReportSheetHeader3,
            ReportingConsts.EventListReportSheetHeader4,
            ReportingConsts.EventListReportSheetHeader5
        };

        var excelRows = new List<List<object>>();
        foreach (var eventItem in events)
        {
            var excelRow = new List<object>
            {
                eventItem.EventName ?? "N/A",
                eventItem.Description ?? "N/A",
                eventItem.Location ?? "N/A",
                eventItem.EventDate.ToString() ?? "N/A",
                eventItem.Community.Name ?? "N/A"
            };
            excelRows.Add(excelRow);
        }

        // create excel file
        using (var stream = new MemoryStream())
        {
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Events");
                worksheet.Column(1).Width = 40;
                worksheet.Column(2).Width = 40;
                worksheet.Column(3).Width = 30;
                worksheet.Column(4).Width = 25;
                worksheet.Column(5).Width = 30;

                for (int i = 0; i < excelHeaders.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = excelHeaders[i];
                }

                for (var r = 0; r < excelRows.Count; r++)
                {
                    for (var i = 0; i < excelRows[r].Count; i++)
                    {
                        var cellValue = excelRows[r][i]?.ToString();
                        worksheet.Cells[r + 2, i + 1].Value=cellValue;
                        worksheet.Cells[r + 2, i + 1].Style.Font.Bold = false;
                        worksheet.Cells[r + 2, i + 1].Style.Font.Size = 12;
                        //OfficeOpenXml.Style.ExcelHorizontalAlignment.Left
                        worksheet.Cells[r + 2, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells[r + 2, i + 1].Style.Font.Size = 12;
                    }
                }
                package.Save();
            }
            stream.Position = 0;
            return stream;
        }
    }
}
