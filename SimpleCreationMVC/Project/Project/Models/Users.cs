
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Users
     {
		[Key]
		public int? Id {get;set;}
		public string? Email {get;set;}
		public string? Username {get;set;}
		public string? HashPassword {get;set;}
		public string? Salt {get;set;}
		public string? FullName {get;set;}
		public bool? IsActive {get;set;}

     }
}
