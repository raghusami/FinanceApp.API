namespace FinanceApp.Application.DTOs
{
    public class IncomeRecordDto
    {
        public int Id { get; set; }
        public DateTime IncomeDate { get; set; }
        public string IncomeSource { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public string? IncomeType { get; set; }
    }
}
