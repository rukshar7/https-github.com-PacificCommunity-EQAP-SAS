using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Security
{
    public class ControllerInfo
    {

        public string Id => $"{AreaName}:{Name}";

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string AreaName { get; set; }

        public IEnumerable<ActionInfo> Actions { get; set; }

    }
}
