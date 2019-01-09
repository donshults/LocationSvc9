using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationSvc.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public int LookupId { get; set; }
        public string LookupValue { get; set; }
        public string LoginName { get; set; }
        public bool IsAdmin { get; set; }
    }
}