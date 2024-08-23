
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Rankings
     {
		[Key]
		public int? Id {get;set;}
		public DateTime? RankingDate {get;set;}
		public string? FacultyRank {get;set;}
		public double? Salary {get;set;}
		public int EmployeeId {get;set;}

     }
}
