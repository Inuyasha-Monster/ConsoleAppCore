using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthApiDemo
{
    public class CustomerSecurityTokenValidator : ISecurityTokenValidator
    {
        public bool CanReadToken(string securityToken)
        {
            return !string.IsNullOrWhiteSpace(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters,
            out SecurityToken validatedToken)
        {
            validatedToken = null;

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

            // 自定义校验token
            if (securityToken == "fuck")
            {
                claimsIdentity.AddClaim(new Claim("name", "djlnet"));
                claimsIdentity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
            }


            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }

        public bool CanValidateToken { get; } = true;
        public int MaximumTokenSizeInBytes { get; set; }
    }
}
