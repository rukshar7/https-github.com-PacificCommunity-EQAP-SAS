using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Base
{
  public   class Outcomes : Entity
    {

        public string Name { get; set; }

        public int UnitStandardId { get; set; }

        public virtual UnitStandards UnitStandards { get; set; }
    }
}
