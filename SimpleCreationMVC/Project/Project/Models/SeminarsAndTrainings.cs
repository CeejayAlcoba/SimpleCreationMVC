
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class SeminarsAndTrainings
     {
		[Key]
		public int? Id {get;set;}
		public bool? IsDelegate {get;set;}
		public bool? IsSpeaker {get;set;}
		public bool? IsOrganizer {get;set;}
		public int EmployeeId {get;set;}

     }
}
