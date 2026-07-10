
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ControlNumber
    {
		[Key]
		public int? Id {get;set;}
		public string? Prefix {get;set;}
		public string? Sequence {get;set;}
		public string? Suffix {get;set;}

    }
}
