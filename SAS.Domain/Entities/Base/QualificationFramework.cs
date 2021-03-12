using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Base
{
   public  class QualificationFramework :Entity
    {

        public int LevelId { get; set; }
        public virtual QualificationLevels QualificationLevels { get; set; }
        public string Name { get; set; }

    }
}
