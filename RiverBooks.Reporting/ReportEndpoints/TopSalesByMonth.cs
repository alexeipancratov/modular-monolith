using FastEndpoints;
using Microsoft.AspNetCore.Mvc;
using RiverBooks.Reporting.Services;

namespace RiverBooks.Reporting.ReportEndpoints;

internal record TopSalesByMonthRequest([FromQuery] int Year, [FromQuery] int Month);
internal record TopSalesByMonthResponse(TopBookByMonthReport Report);

internal class TopSalesByMonth(ITopSellingBooksReportService topSellingBooksReportService)
  : Endpoint<TopSalesByMonthRequest, TopSalesByMonthResponse>
{
  private readonly ITopSellingBooksReportService _topSellingBooksReportService = topSellingBooksReportService;

  public override void Configure()
  {
    Get("/topSales");
    AllowAnonymous(); // TODO: lock down
  }

  public override async Task HandleAsync(TopSalesByMonthRequest req, CancellationToken ct)
  {
    var report = await _topSellingBooksReportService.ReachInSqlQuery(req.Month, req.Year);
    var response = new TopSalesByMonthResponse(report);
    
    await SendAsync(response, 200, ct);
  }
}
