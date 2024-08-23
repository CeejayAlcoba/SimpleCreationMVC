
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Contracts
     {
		[Key]
		public int? Id {get;set;}
		public DateTime? DateStarted {get;set;}
		public DateTime? DateEnded {get;set;}
		public bool? IsCleared {get;set;}
		public int EmployeeId {get;set;}

     }
}
