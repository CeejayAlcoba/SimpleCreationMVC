
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class EducationalDatas
     {
		[Key]
		public int? Id {get;set;}
		public string? ElemSchool {get;set;}
		public int? ElemYearGrad {get;set;}
		public string? HighSchool {get;set;}
		public int? HighSchoolYearGrad {get;set;}
		public string? SHSchool {get;set;}
		public int? SHSchoolYearGrad {get;set;}
		public string? College {get;set;}
		public int? CollegeYearGrad {get;set;}
		public string? MA {get;set;}
		public int? MAYearGrad {get;set;}
		public string? Doctorate {get;set;}
		public int? DoctorateYearGrad {get;set;}
		public string? Techvoc {get;set;}
		public int? TechvocYearGrad {get;set;}
		public string? OtherStudies {get;set;}
		public int? OtherStudiesYearGrad {get;set;}
		public string? License {get;set;}
		public string? ExaminationAndRating {get;set;}
		public string? HonorsAndAwards {get;set;}
		public int EmployeeId {get;set;}

     }
}
