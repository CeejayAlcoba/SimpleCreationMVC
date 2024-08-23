
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class ExternalWorkExperiences
     {
		[Key]
		public int? Id {get;set;}
		public string? Position {get;set;}
		public string? Employer {get;set;}
		public string? LengthOfService {get;set;}
		public string? ReasonForLeaving {get;set;}
		public int EmployeeId {get;set;}

     }
}
