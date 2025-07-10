namespace FinanceApp.Shared.Exceptions
{
    public class FinanceValidationException : Exception
    {
        public FinanceValidationException(string message) : base(message) { }
    }
}
