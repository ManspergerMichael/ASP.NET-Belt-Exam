using System.ComponentModel.DataAnnotations;
using System;
namespace Belt_Exam.Models{
    public class User{
        [Key]
        public int UserID{get;set;}
        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]*$")]
        public string FirstName{get;set;}

        [Required]
        [MinLength(3)]
        [RegularExpression("^[a-zA-Z]*$")]
        public string LastName{get;set;}
        [Required]
        [EmailAddress]
        
        public string Email{get;set;}
        [RegularExpression("^(?=(.*[a-zA-Z].*){2,})(?=.*\\d.*)(?=.*\\W.*)[a-zA-Z0-9\\S]{8,15}$",
            ErrorMessage ="Password must be between 8-15 characters, and have one capital letter, one number, and one special character")]
        public string Password{get;set;}

        
    }
}