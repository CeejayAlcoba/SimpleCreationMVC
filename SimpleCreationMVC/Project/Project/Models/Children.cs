
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Children
     {
		[Key]
		public int? Id {get;set;}
		public string? Name {get;set;}
		public DateTime? Bday {get;set;}
		public string? GradeLevel {get;set;}
		public int FamilyId {get;set;}

     }
}
