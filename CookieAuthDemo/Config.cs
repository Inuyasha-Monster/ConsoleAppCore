using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace CookieAuthDemo
{
    public class Config
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "mvc",
                    ClientName = "mvc Client",
                    ClientUri="http://localhost:5002",

                    LogoUri = "https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=2904524590,3613205869&fm=27&gp=0.jpg",

                    AllowRememberConsent = true,

                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets = new List<Secret>()
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>()
                    {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    },
                    RedirectUris = {"http://localhost:5002/signin-oidc"},
                    PostLogoutRedirectUris = {"http://localhost:5002/signout-callback-oidc"},
                    //RequireConsent = false
                    RequireConsent = true
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api1","API Application")
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser()
                {
                    SubjectId = "10000",
                    Username = "djlnet",
                    Password = "111111",
                    
                    Claims = new []
                    {
                        new Claim("name", "djlnet"),
                        new Claim("website", "https://djlnet.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "fuck",
                    Password = "111111",
                    Claims = new []
                    {
                        new Claim("name", "fuck"),
                        new Claim("website", "https://fuck.com")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}
