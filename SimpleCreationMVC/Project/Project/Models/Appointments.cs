
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Appointments
     {
		[Key]
		public int? Id {get;set;}
		public string? NatureOfAppointment {get;set;}
		public double? Pay {get;set;}
		public string? PeriodOfAppointment {get;set;}
		public int InternalWorkExperienceId {get;set;}

     }
}
