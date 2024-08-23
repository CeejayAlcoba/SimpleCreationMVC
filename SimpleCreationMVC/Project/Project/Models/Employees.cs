
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Employees
     {
		[Key]
		public int? Id {get;set;}
		public string FirstName {get;set;}
		public string? MiddleName {get;set;}
		public string LastName {get;set;}
		public DateTime? DateHire {get;set;}
		public string? Gender {get;set;}
		public string? CivilStatus {get;set;}
		public string? Religion {get;set;}
		public DateTime? Birthdate {get;set;}
		public string? PlaceOfBirth {get;set;}
		public string? PresentAddress {get;set;}
		public string? PermanentAddress {get;set;}
		public string? NationalIDNumber {get;set;}
		public string? SSSNumber {get;set;}
		public string? HDMFNumber {get;set;}
		public string? PHICNumber {get;set;}
		public string? TIN {get;set;}
		public string? HMOCardNumber {get;set;}
		public string? HMOAccountNumber {get;set;}
		public bool? IsApproved {get;set;}
		public string? ContactNo {get;set;}
		public string? EmailAddress {get;set;}
		public int? PositionId {get;set;}

     }
}
