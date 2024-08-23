
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class FamilyDatas
     {
		[Key]
		public int? Id {get;set;}
		public string? MotherMaidenName {get;set;}
		public string? FatherName {get;set;}
		public string? Spouse {get;set;}
		public int EmployeeId {get;set;}

     }
}
