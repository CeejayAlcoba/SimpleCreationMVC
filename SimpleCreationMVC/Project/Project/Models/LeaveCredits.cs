
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class LeaveCredits
     {
		[Key]
		public int? Id {get;set;}
		public int? SickLeave {get;set;}
		public int? VacationLeave {get;set;}
		public DateTime? AnniversaryDate {get;set;}
		public int CorporateDataId {get;set;}

     }
}
