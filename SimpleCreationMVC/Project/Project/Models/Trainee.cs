
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Trainee
    {
		[Key]
		public int? Id {get;set;}
		public string? FirstName {get;set;}
		public string? LastName {get;set;}
		public string? SerialNumber {get;set;}

    }
}
