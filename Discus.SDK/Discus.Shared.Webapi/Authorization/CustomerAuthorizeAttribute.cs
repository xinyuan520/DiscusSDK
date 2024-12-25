using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.Shared.WebApi.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CustomerAuthorizeAttribute : AuthorizeAttribute
    {
        public string[] AuthCode { get; set; }

        public CustomerAuthorizeAttribute(string authCode, string schemes = JwtBearerDefaults.AuthenticationScheme) : this(new string[] { authCode }, schemes)
        {

        }

        public CustomerAuthorizeAttribute(string[] codes, string schemes = JwtBearerDefaults.AuthenticationScheme)
        {
            AuthCode = codes;
            Policy = AuthorizePolicy.Policy;
            if (string.IsNullOrWhiteSpace(schemes))
                throw new ArgumentNullException(nameof(schemes));
            else
                AuthenticationSchemes = schemes;
        }
    }
}
