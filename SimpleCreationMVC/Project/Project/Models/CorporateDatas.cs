
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class CorporateDatas
     {
		[Key]
		public int? Id {get;set;}
		public string? Salutation {get;set;}
		public string? Department {get;set;}
		public string? ReportingTo {get;set;}
		public string? Category {get;set;}
		public DateTime? RegularizationDate {get;set;}
		public bool? IsTaxExempt {get;set;}
		public bool? WaiveContributions {get;set;}
		public double? BasicPay {get;set;}
		public double? Allowance {get;set;}
		public double? Honorarium {get;set;}
		public double? PartTimePay {get;set;}
		public string? TimeShift {get;set;}
		public string? PayrollAccountNumber {get;set;}
		public string? ValidSignature {get;set;}
		public int EmployeeId {get;set;}

     }
}
