using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.Entities
{
    public enum AccreditationStatus
    {
        Approved,
        Provisional,
        Revoked,
        Recognised

    }

    public enum Status

    {
        Active,
        InActive
    }


    public enum Gender

    {
        Male,
        Female
    }


    public enum EnrollmentStatus
    {
        Enrolled,
        Withdrawn,
        Transferred,
        Repeated,
        Pending
    }

    public enum AssessorType
    {
        Trainer,
        HOD
    }

}
