
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class CommunityInvolvements
     {
		[Key]
		public int? Id {get;set;}
		public bool? IsChair {get;set;}
		public bool? IsMember {get;set;}
		public int EmployeeId {get;set;}

     }
}
