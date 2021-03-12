using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities.Base
{
  public  class Institutions :Entity
    {

        public string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string RegistrationNo { get; set; }

        public AccreditationStatus AccreditationStatus { get; set; }

        


    }
}
