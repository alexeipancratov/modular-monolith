using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using RiverBooks.Reporting.Services;

namespace RiverBooks.Reporting.ReportEndpoints;

internal record TopSalesByMonth2Request([FromQuery] int Year, [FromQuery] int Month);
internal record TopSalesByMonth2Response(TopBooksByMonthReport Report);

internal class TopSalesByMonth2(ISalesReportService salesReportService)
  : Endpoint<TopSalesByMonthRequest, TopSalesByMonthResponse>
{
  private readonly ISalesReportService _salesReportService = salesReportService;

  public override void Configure()
  {
    Get("/topSales2");
    AllowAnonymous(); // TODO: lock down
  }

  public override async Task HandleAsync(TopSalesByMonthRequest req, CancellationToken ct)
  {
    var report = await _salesReportService.GetTopBooksByMonthReportAsync(req.Month, req.Year);
    var response = new TopSalesByMonthResponse(report);
    
    await SendAsync(response, 200, ct);
  }
}
