
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class PublishedBooks
     {
		[Key]
		public int? Id {get;set;}
		public string? Title {get;set;}
		public int EmployeeId {get;set;}

     }
}
