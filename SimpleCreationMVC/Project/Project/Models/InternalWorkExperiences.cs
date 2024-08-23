
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class InternalWorkExperiences
     {
		[Key]
		public int? Id {get;set;}
		public string? Position {get;set;}
		public string? Department {get;set;}
		public string? JobLevel {get;set;}
		public DateTime? DateStart {get;set;}
		public DateTime? DateEnd {get;set;}
		public int EmployeeId {get;set;}

     }
}
