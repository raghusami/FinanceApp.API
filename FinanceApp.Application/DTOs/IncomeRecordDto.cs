namespace FinanceApp.Application.DTOs
{
    public class IncomeRecordDto
    {
        public int Id { get; set; }
        public string IncomeDate { get; set; } = string.Empty;
        public string IncomeSource { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public string? IncomeType { get; set; }
    }
}
