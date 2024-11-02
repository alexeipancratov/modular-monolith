using System.Globalization;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RiverBooks.Reporting.Services;

internal class TopSellingBooksReportService : ITopSellingBooksReportService
{
  private readonly string _connectionString;
  private readonly ILogger<TopSellingBooksReportService> _logger;

  public TopSellingBooksReportService(
    IConfiguration configuration,
    ILogger<TopSellingBooksReportService> logger)
  {
    _logger = logger;
    _connectionString = configuration.GetConnectionString("OrderProcessingConnectionString")!;
  }
  
  public async Task<TopBookByMonthReport> ReachInSqlQuery(int month, int year)
  {
    const string sql = """
                       SELECT b.Id, b.Title, b.Author, SUM(oi.Quantity) as Units,
                       SUM(oi.UnitPrice * oi.Quantity) as Sales
                       FROM Books.Books b
                       INNER JOIN OrderProcessing.OrderItem oi ON b.id = oi.BookId
                       INNER JOIN OrderProcessing.Orders o ON o.Id = oi.OrderId
                       WHERE MONTH(o.DateCreated) = 9 AND YEAR(o.DateCreated) = 2024
                       GROUP BY b.Id, b.Title, b.Author
                       ORDER BY Sales DESC
                       """;

    await using var conn = new SqlConnection(_connectionString);
    _logger.LogInformation("Executing query {sql}", sql);
    var results = await conn.QueryAsync<BookSalesResult>(sql, new { month, year });

    var report = new TopBookByMonthReport
    {
      Month = month,
      Year = year,
      MonthName = CultureInfo.GetCultureInfo("en-US").DateTimeFormat.GetMonthName(month),
      Results = results.ToList()
    };

    return report;
  }
}
