using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Base
{
    public class NationalQualifications
    {

        public string Name { get; set; }

        public int LevelId { get; set; }

        public virtual QualificationLevels QualificationLevels { get; set; }

        public string NQCode { get; set; }

        public Status Status { get; set; }
    }
}
