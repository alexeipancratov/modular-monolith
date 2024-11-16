namespace RiverBooks.Reporting.Services;

internal interface ISalesReportService
{
  Task<TopBooksByMonthReport> GetTopBooksByMonthReportAsync(int month, int year);
}
