namespace RiverBooks.Reporting;

public class TopBookByMonthReport
{
  public int Year { get; set; }

  public int Month { get; set; }

  public string MonthName { get; set; } = string.Empty;

  public List<BookSalesResult> Results { get; set; } = [];
}
