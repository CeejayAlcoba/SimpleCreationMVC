
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Authority
    {
		[Key]
		public int? Id {get;set;}
		public int? PersonnelId {get;set;}
		public string? Name {get;set;}
		public bool? IsApproved {get;set;}

    }
}
