using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNU.Internet.WebParts
{
    public class tblOrganizationLevel1
    {
        public int ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
    public class tblOrganizationLevel2
    {
        public int ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int FK_OrganizationLevel1 { get; set; }
    }
    public class tblOrganizationLevel3
    {
        public int ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int FK_OrganizationLevel2ID { get; set; }
    }
    public class tblOrganizationLevel4
    {
        public int ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int FK_OrganizationLevel3ID { get; set; }
    }
    public class tblJobs
    {
        public int ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? FK_OrganizationLevel1ID { get; set; }
        public int? FK_OrganizationLevel2ID { get; set; }
        public int? FK_OrganizationLevel3ID { get; set; }
        public int? FK_OrganizationLevel4ID { get; set; }

        public string JobGradeAr { get; set; }
    }
    public class tblEmployees
    {
        public int ID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeFullNameAr { get; set; }
        public string EmployeeFullNameEn { get; set; }
        public string EmployeeNameEn { get; set; }
        public int? FK_JobID { get; set; }
        public int? FK_OrganizationLevel1ID { get; set; }
        public int? FK_OrganizationLevel2ID { get; set; }
        public int? FK_OrganizationLevel3ID { get; set; }
        public string LineManager { get; set; }
        public string ProbationPeriodStatus { get; set; }
        //public DateTime? HireDate { get; set; }
        public string HireDate { get; set; }
        public string Email { get; set; }
        public string Extensions { get; set; }
        public string Status { get; set; }
        public string FirstNameEn { get; set; }
        public string LastNameEn { get; set; }
        public string NationalID { get; set; }
        public string PersonalEmail { get; set; }
        public string MobileNo { get; set; }
    }
    public class vwEmployees
    {
        public int? ID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeFullNameAr { get; set; }
        public string EmployeeFullNameEn { get; set; }
        public string EmployeeNameEn { get; set; }
        public int? JobID { get; set; }
        public string JobNameAr { get; set; }
        public string JobNameEn { get; set; }
        public int? OrganizationLevel1ID { get; set; }
        public string OrganizationLevel1NameAr { get; set; }
        public string OrganizationLevel1NameEn { get; set; }
        public int? OrganizationLevel2ID { get; set; }
        public string OrganizationLevel2NameAr { get; set; }
        public string OrganizationLevel2NameEn { get; set; }
        public int? OrganizationLevel3ID { get; set; }
        public string OrganizationLevel3NameAr { get; set; }
        public string OrganizationLevel3NameEn { get; set; }
        public string LineManager { get; set; }
        public string ProbationPeriodStatus { get; set; }
        public DateTime? HireDate { get; set; }
        public string Email { get; set; }
        public string Extensions { get; set; }
        public string Status { get; set; }
        public string FirstNameEn { get; set; }
        public string LastNameEn { get; set; }
        public string NationalID { get; set; }
        public string PersonalEmail { get; set; }
        public string MobileNo { get; set; }
        public string JobGradeAr { get; set; }
    }


    public class vw_JobGradeAr
    {
        public string JobGradeAr { get; set; }
    }

    public class vw_ProbationPeriods
    {
        public string ProbationPeriodStatus { get; set; }
    }


}
