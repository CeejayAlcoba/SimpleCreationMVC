
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class ProfessionalAffiliations
     {
		[Key]
		public int? Id {get;set;}
		public bool? IsMember {get;set;}
		public bool? IsOfficer {get;set;}
		public int EmployeeId {get;set;}

     }
}
