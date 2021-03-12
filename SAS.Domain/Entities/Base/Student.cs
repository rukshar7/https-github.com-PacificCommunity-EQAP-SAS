using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Base
{
    public class Student:Entity
    {

        public int StudentId { get; set; }
        public string Surname { get; set; }
        public string OtherNames { get; set; }

       
        public string Ethnicity { get; set; }

        public Gender Gender { get; set; }

        public DateTime DoB { get; set; }

        public string ResidentalAddress { get; set; }

        public string PostalAdress { get; set; }

        public int Contact { get; set; }

        public string Email { get; set; }
       
        public virtual IList<Enrollment> Enrollments { get; set; }
    }
}
