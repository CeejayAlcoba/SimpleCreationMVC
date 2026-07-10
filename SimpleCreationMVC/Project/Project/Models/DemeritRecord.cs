
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class DemeritRecord
    {
		[Key]
		public int? Id {get;set;}
		public int? ControlNumberId {get;set;}
		public int? TraineeId {get;set;}
		public DateTime? DateReceived {get;set;}
		public DateTime? DateTimeSubmitted {get;set;}
		public int? AuthorityId {get;set;}
		public int? AutorityId {get;set;}
		public double? Demerit {get;set;}
		public double? TouringHours {get;set;}
		public double? Confinement {get;set;}

    }
}
