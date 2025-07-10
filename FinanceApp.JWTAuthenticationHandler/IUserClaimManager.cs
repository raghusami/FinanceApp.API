namespace FinanceApp.JWTAuthenticationHandler
{
    public interface IUserClaimManager
    {

        public string UserName { get; }

        public string UserEmailId { get; }

        public int UserUniqueId { get; }
    }
}
