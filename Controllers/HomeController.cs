using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Part1ex.Models;
using System.Collections.Generic;

namespace Part1ex.Controllers
{
    //create an im-memeory storage for incoming and new claims which is temporary
    public class HomeController : Controller
    {
       private static List<Calculations> claimsList = new List<Calculations>(); 
    
    
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //action method to navigate to the register page.
        //THE Iaction methid is very important to have the view method after having a viewpage
        /* public IActionResult Register() 

         { 
             return View();
         }
        */
        //we first check that the information is provided first so hence the null , then add the user into the rolesdown list sue swith to send the user to the corrct dashbpard 
        public IActionResult Register(string Name = null, string Email = null, string Password = null, string Role = null)
        {
            if (!string.IsNullOrEmpty(Name)  &&
                !string.IsNullOrEmpty(Email) &&
                !string.IsNullOrEmpty(Password) &&
                !string.IsNullOrEmpty(Role))
            {
                RolesDown.Roles.Add(new Rol
                {
                    Name = Name,
                    Email = Email,
                    Password = Password,
                    Role = Role
                });

                TempData["UserRole"] = Role;
                TempData["UserName"] = Name;

                return Role switch
                {
                    "Lecturer" =>
                        RedirectToAction("LecturerDashboard"),
                    "program Coordinator" =>
                        RedirectToAction("CoordinatorDashboard"),
                    "ProgramManager" =>
                        RedirectToAction("ManagerDashboard"),
                    _ => RedirectToAction("UserBoard")
                };
            }
            return View();
        }


        //action method to navigate to the dashboard page

        public IActionResult Dashboard()
        {
            return View();
        }

        //This is the claims action method to navigae to the claims and the claims dashboard
        //lets test how the one single IActionResult work and if it does catch  exceptions
        //we make sure that the user doesnt enter null values and every value is bigger than 0
        //and we calculate the hourly rate of the hours worked and submit the final value to them
        public IActionResult Claims(Calculations? model ,IFormFile uploadFile)
        {
            if (model !=null && model.HoursWorked >0 && model.HourlyRate >0)
            {
                    model.TotalAmount = model.HoursWorked * model.HourlyRate;
                    model.ClaimDate = DateTime.Now;
                model.ClaimStatus = "pending";

                //then we add to a list 
                claimsList.Add(model);
                
                
                return View(model);
            }
            return View(new Calculations());
        }


        //This is the claims action method to navigae to the claims and the claims dashboard
        /*
        public IActionResult Claims()
        {
            return View(new Calculations());
        }

        //then we calculate the total claims that the user has enetred with there hourly rate 

        public IActionResult Claims(Calculations model)
        { 
        
            if (ModelState.IsValid)
            {
                model.TotalAmount = model.HoursWorked * model.HourlyRate;
            }
            return View(model);
        }
        */

        public IActionResult Claims_Dashboard()
        {
           //var claimsList = new List<Calculations>();
            return View(claimsList);
        }

        //thehn here we make sure that the documents are uploaded and it taks us back to the claims
        public IActionResult UploadDocument(IFormFile uploadFile)
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {

                TempData["UploadMessage"] = $"File'{uploadFile.FileName}'upload successfully.";

            }
            else
            {
                TempData["Upload Message"] = "No file Selected.";
            }
            return  RedirectToAction("Claims");
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
