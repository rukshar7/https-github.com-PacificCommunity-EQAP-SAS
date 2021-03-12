using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Security
{
    public class ActionInfo
    {
        public int ActionId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string ControllerId { get; set; }

    }
}
