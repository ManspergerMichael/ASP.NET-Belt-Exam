using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belt_Exam.Models{
    public class UserActivity{
        [Key]
        public int ActivityID{get;set;}
        [Required]
        [MinLength(2)]
        public string Title{get;set;}
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy", ApplyFormatInEditMode = true)]
        public DateTime Date{get;set;}

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Time{get;set;}

        [Required]
        public int Duration{get;set;}
        public string DurationUnits{get;set;}
        [Required]
        [MinLength(10)]
        public string Description{get;set;}

        [ForeignKey("User")]
        public int UserID{get;set;}
        public User User{get;set;}

        public List<Attendee> Attendees{get;set;}

        public UserActivity(){
            Attendees = new List<Attendee>();
        }

    }
}