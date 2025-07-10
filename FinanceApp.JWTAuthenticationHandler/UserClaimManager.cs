using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FinanceApp.JWTAuthenticationHandler
{
    public class UserClaimManager : IUserClaimManager
    {
        private readonly IHttpContextAccessor contextAccessor;

        public UserClaimManager(IHttpContextAccessor _IHttpContextAccessor)
        {
            contextAccessor = _IHttpContextAccessor;
        }

     

        public string UserName
        {
            get
            {
                return contextAccessor?.HttpContext?.User?.FindFirstValue("UserName");
            }
        }


        public string UserEmailId
        {
            get
            {
                return contextAccessor?.HttpContext?.User?.FindFirstValue("UserEmail");
            }
        }

        public int UserUniqueId
        {
            get
            {
                return Convert.ToInt32(contextAccessor?.HttpContext?.User?.FindFirstValue("UserUniqueId"));
            }
        }
     
    }
}
