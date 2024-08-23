
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class PaperResearchPresentations
     {
		[Key]
		public int? Id {get;set;}
		public bool? IsNational {get;set;}
		public bool? IsInternational {get;set;}
		public int EmployeeId {get;set;}

     }
}
