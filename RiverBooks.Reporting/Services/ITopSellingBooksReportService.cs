namespace RiverBooks.Reporting.Services;

internal interface ITopSellingBooksReportService
{
  /// <summary>
  /// Retrieves data for reporting purposes using the Reach In AntiPattern.
  /// </summary>
  /// <remarks>It's an AntiPattern since we depend on source DB schema
  /// AND we even assume that both Books and Books orders live in the same DB.</remarks>
  /// <param name="month"></param>
  /// <param name="year"></param>
  /// <returns>Report data.</returns>
  Task<TopBooksByMonthReport> ReachInSqlQuery(int month, int year);
}
