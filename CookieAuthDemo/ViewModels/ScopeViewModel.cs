using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookieAuthDemo.ViewModels
{
    public class ScopeViewModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Emphasize { get; set; }
        public string Required { get; set; }

        public bool Checked { get; set; }
    }
}
