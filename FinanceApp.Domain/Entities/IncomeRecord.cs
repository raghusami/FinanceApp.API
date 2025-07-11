namespace FinanceApp.Domain.Entities
{
    public class IncomeRecords
    {
        public int Id { get; set; }
        public DateTime IncomeDate { get; set; }
        public string IncomeSource { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? IncomeType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
