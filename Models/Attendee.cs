using System.ComponentModel.DataAnnotations;
using System;
namespace Belt_Exam.Models{
    public class Attendee{
        [Key]
        public int AttendeeID{get;set;}

        public int ActivityID{get;set;}
        public UserActivity UserActivity{get;set;}

        public int UserID{get;set;}
        public User User{get;set;}
    }
}