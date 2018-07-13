using Microsoft.EntityFrameworkCore; 
namespace Belt_Exam.Models{
    public class UserContext : DbContext{
        public UserContext(DbContextOptions<UserContext> options): base(options){}
       public DbSet<User> Users{get;set;}
       public DbSet<UserActivity> Activities{get;set;}
       public DbSet<Attendee> Attendees{get;set;} 
    }
}