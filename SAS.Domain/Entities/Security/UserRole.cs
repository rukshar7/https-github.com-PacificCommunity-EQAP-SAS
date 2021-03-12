using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Security
{
    public class UserRole
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool IsApproved { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }

    }
}
