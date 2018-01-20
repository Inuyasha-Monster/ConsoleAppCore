using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookieAuthDemo.ViewModels
{
    public class ProcessConsentResult
    {
        public string ReturnUrl { get; set; }
        public bool IsRedirect => !string.IsNullOrWhiteSpace(ReturnUrl);

        public string ValidationError { get; set; }

        public ConsentViewModel ConsentViewModel { get; set; }
    }
}
