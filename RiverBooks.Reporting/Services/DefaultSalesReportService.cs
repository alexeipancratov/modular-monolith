using System.Globalization;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RiverBooks.Reporting.Services;

internal class DefaultSalesReportService(
  IConfiguration config,
  ILogger<DefaultSalesReportService> logger)
  : ISalesReportService
{
  private readonly ILogger<DefaultSalesReportService> _logger = logger;
  private readonly string _connString = config.GetConnectionString("ReportingConnectionString")!;

  public async Task<TopBooksByMonthReport> GetTopBooksByMonthReportAsync(int month, int year)
  {
    // NOTE: a very simple query (no aggregation or filtering as it was already done
    // when we populated the Reporting DB).
    const string sql = """
                       select BookId, Title, Author, UnitsSold as Units, TotalSales as Sales
                       from Reporting.MonthlyBookSales
                       where Month = @month and Year = @year
                       ORDER BY TotalSales DESC
                       """;
    await using var conn = new SqlConnection(_connString);
    _logger.LogInformation("Executing query: {sql}", sql);
    var results = (await conn.QueryAsync<BookSalesResult>(sql, new { month, year }))
      .ToList();

    var report = new TopBooksByMonthReport
    {
      Year = year,
      Month = month,
      MonthName = CultureInfo.GetCultureInfo("en-US").DateTimeFormat.GetMonthName(month),
      Results = results
    };

    return report;
  }
}
