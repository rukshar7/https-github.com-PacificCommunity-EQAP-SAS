using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Base
{
    public class UnitStandards :Entity
    {

        public string Name { get; set; }
        public string Code { get; set; }

        public int LevelId { get; set; }

        public virtual QualificationLevels QualificationLevels { get; set; }


        public int Credits { get; set; }

        public int NationalQualificationId { get; set; }

        public virtual NationalQualifications NationalQualifications { get; set; }
    }
}
