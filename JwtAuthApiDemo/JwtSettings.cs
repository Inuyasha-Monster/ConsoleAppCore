using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthApiDemo
{
    public class JwtSettings
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string IssuerSigningKey { get; set; }
    }
}
