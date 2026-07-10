
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class TouringDeduction
    {
		[Key]
		public int? Id {get;set;}
		public double? Hours {get;set;}
		public DateTime? Date {get;set;}
		public int? DemeritRecordId {get;set;}

    }
}
