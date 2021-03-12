using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Base
{
    public class NQAdministration :Entity
    {

        public int InstitutionId { get; set; }

        public virtual Institutions Institutions { get; set; }

        public int NationalQualificationId { get; set; }

        public virtual NationalQualifications NationalQualification { get; set; }

       // public int Year { get; set; }
    }
}
