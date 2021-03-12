using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Base
{
    public class Enrollment :Entity
    {

        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public EnrollmentStatus Status { get; set; } 
        public int AcademicYear { get; set; }

        public DateTime EnrollmentDate { get; set; }

        public int InstitutionId { get; set; }

        public virtual Institutions Institutions { get; set; }

        public int NQAdministrationId { get; set; }
         
        public virtual NQAdministration NQAdministration { get; set; }

    }
}
