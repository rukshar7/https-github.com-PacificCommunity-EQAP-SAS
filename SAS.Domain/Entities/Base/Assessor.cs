using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Base
{
   public  class Assessor :Entity
    {

        public int AssessorId { get; set; }

        public string Surname { get; set; }
        public string OtherNames { get; set; }

        public string Ethnicity { get; set; }

        public Gender Gender { get; set; }

        public DateTime DoB { get; set; }

        public string TradeQualification { get; set; }

        public string IndustryExperience { get; set; }

        public string YearsTeaching { get; set; }

        public string MedicalConditions { get; set; }

        public string ResidentalAddress { get; set; }

        public string PostalAdress { get; set; }

        public int Contact { get; set; }

        public string Email { get; set; }

        public AssessorType AssessorType { get; set; }
    }
}
