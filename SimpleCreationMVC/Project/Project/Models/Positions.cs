
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Positions
     {
		[Key]
		public int? Id {get;set;}
		public string? Description {get;set;}
		public bool? IsActive {get;set;}
		public bool? IsDeletable {get;set;}

     }
}
