using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Belt_Exam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Belt_Exam.Controllers
{
    public class HomeController : Controller
    {
       private UserContext _context;
        public HomeController(UserContext context){
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Home(){
            
            return View("Login");
        }
        [HttpPost("Register")]
        public IActionResult Register(User user, string ConfirmPassword){
            User userEmail = _context.Users.SingleOrDefault(login => login.Email == user.Email);
            if(user.Password == ConfirmPassword){
                if(userEmail == null){
                    if(ModelState.IsValid){
                        PasswordHasher<User> Hasher = new PasswordHasher<User>();
                        user.Password = Hasher.HashPassword(user, user.Password);
                        _context.Add(user);
                        _context.SaveChanges();
                        User userSession = _context.Users.SingleOrDefault(u => u.Email == user.Email);
                        HttpContext.Session.SetInt32("UserID",user.UserID);
                        return RedirectToAction("DashBoard");
                    }
                    else{
                        return View("Login");
                    }
                }
                else{
                    ViewBag.RegistrationError = "Email is in use.";
                    return View("Login");
                }
            }
            else{
                ViewBag.CPError = "Passwords do no match.";
                return View("Login");
            }

        }

        [HttpPost("Login")]
        public IActionResult Login(string email, string Password){
            User user = _context.Users.SingleOrDefault(u => u.Email == email);
            if(user != null && Password != null){
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(user, user.Password, Password)){
                    HttpContext.Session.SetInt32("UserID",user.UserID);
                    return RedirectToAction("DashBoard");
                }
                else{
                    ViewBag.loginError = "Invalid Password";
                    return View("Login");
                }
            }
            else{
                ViewBag.loginError = "Email not registered";
                return View("Login");
            }
            
        }
        [HttpGet("Dashboard")]
        public IActionResult DashBoard(){
            //get logged in user
            int Uid = (int)HttpContext.Session.GetInt32("UserID");
            User user = _context.Users.SingleOrDefault(u => u.UserID == Uid);
            ViewBag.User = user;
            ViewBag.SessionUser = Uid;
            //get list of activities
            List<UserActivity> activities = _context.Activities.
                Include(a =>a.Attendees).
                    Include(a =>a.User).OrderByDescending(a => a.Date).ToList();
            ViewBag.Activities = activities;
            return View("Dashboard");
        }
        [HttpGet("Logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Home");
        }
        [HttpGet("NewActivity")]
        public IActionResult NewActivity(){
            return View();
        }

        [HttpPost("/Home/NewActivityProcess")]
        public IActionResult NewActivity(UserActivity activity){
            int Uid = (int)HttpContext.Session.GetInt32("UserID");

            if(ModelState.IsValid){
                activity.UserID = Uid;
                _context.Activities.Add(activity);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else{
                return View("NewActivity");
            }
        }
        [HttpGet("details/{Aid}")]
        public IActionResult details(int Aid){
            int Uid = (int)HttpContext.Session.GetInt32("UserID");
            UserActivity activity = _context.Activities.
                Include(a => a.User).
                    Include(a => a.Attendees).
                        ThenInclude(a => a.User).
                            SingleOrDefault(a => a.ActivityID == Aid);
            ViewBag.SessionUser = Uid;
            return View("Details", activity);
        }
        [HttpGet("/details/Join/{Uid}/{Aid}")]
        public IActionResult DetailJoin(int Uid, int Aid){
            Attendee attendee = new Attendee();
            attendee.ActivityID = Aid;
            attendee.UserID = Uid;
            _context.Attendees.Add(attendee);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet("cancel/{Aid}")]
        public IActionResult cancel(int Aid){
            //check if user id in session is the same as the user id for the event
            //prevents users from deleting events they didnt create
            UserActivity activity = _context.Activities.SingleOrDefault(a => a.ActivityID == Aid);
            List<Attendee> attendees = _context.Attendees.Where(a => a.ActivityID == Aid).ToList();
            _context.Attendees.RemoveRange(attendees);
            _context.Activities.Remove(activity);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet("Join/{Uid}/{Aid}")]
        public IActionResult DashJoin(int Uid, int Aid){
            Attendee attend = new Attendee();
            attend.UserID = Uid;
            attend.ActivityID = Aid;
            _context.Attendees.Add(attend);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("Leave/{Uid}/{Aid}")]
        public IActionResult DashLeave(int Uid, int Aid){
            List<Attendee> attend = _context.Attendees.Where(a => a.UserID == Uid).Where(a =>a.ActivityID == Aid).ToList();
            _context.Attendees.RemoveRange(attend);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
