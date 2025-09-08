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

            if(!claimsList.Any())
            {
                claimsList.AddRange(TestClaims.Claims);
            }
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
        //then we can make sure that the username and details that they entered are correct if not error messege appears
        public IActionResult Login(string Email, string Password, string role = null)
        {
            /*var user = RolesDown.Roles.FirstOrDefault(mem => mem.Email == Email && mem.Password == Password);
        
            if (user !=null)
            {
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserName", user.Name);

                return user.Role switch
                {

                    "Lecturer" => RedirectToAction("Dashboard"),
                    "Program Coordinator" => RedirectToAction("CoordinatorDashboard"),
                    "Program Manager" => RedirectToAction("ManagerDashboard"),
                    _ => RedirectToAction("Dashboard")
                };
            }
            TempData["LoginError"] = "Invalid email or password ";
            return RedirectToAction("Index");
*/
            if (!string.IsNullOrEmpty(role))
            {
                HttpContext.Session.SetString("UserRole", role);
                HttpContext.Session.SetString("UserName", $"Test{role}");
                return role switch
                {
                    "Lecturer" => RedirectToAction("Dashboard"),
                    "Program Coordinator" => RedirectToAction("CoordinatorDashboard"),
                    "Program Manager" => RedirectToAction("ManagerDashboard"),
                    _ => RedirectToAction("Dashboard")
                };
            }

            TempData["LoginError"] = "Invalid email , password or choose role. ";
            return RedirectToAction("Index");

        }


        public IActionResult Register(string Name = null, string Email = null, string Password = null)
        {
            if (Request.Method == "POST") { //can be able to redirect to register 

            TempData["SuccessMessage"] = "Registration Susccessful . please login";
            return RedirectToAction("Index");


                /* if (!string.IsNullOrEmpty(Name) &&
                     !string.IsNullOrEmpty(Email) &&
                     !string.IsNullOrEmpty(Password))
                 {
                )
                HttpContext.Session.SetString("UserRole", Role);
                HttpContext.Session.SetString("UserName", Name);

                return Role switch
                {
                    "Lecturer" => RedirectToAction("Dashboard"),
                "Program Coordinator" => RedirectToAction("CoordinatorDashboard"),
                "Program Manager" => RedirectToAction("ManagerDashboard"),
                        _ => RedirectToAction("Dashboard")
                    };
                }
                    return View();
                }
                */
            }

            return View();
        }




        //action method to navigate to the dashboard page

        public IActionResult Dashboard()
        {

            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Lecturer") return RedirectToAction("Index");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View();
        }

        //This is the claims action method to navigae to the claims and the claims dashboard
        //lets test how the one single IActionResult work and if it does catch  exceptions
        //we make sure that the user doesnt enter null values and every value is bigger than 0
        //and we calculate the hourly rate of the hours worked and submit the final value to them

        private static int nextClaimId = 1;
        
        public IActionResult Claims(Calculations? model ,IFormFile uploadFile)
        {
            if (model != null && model.HoursWorked > 0 && model.HourlyRate > 0)
            {
                model.TotalAmount = model.HoursWorked * model.HourlyRate;
                model.ClaimDate = DateTime.Now;
                model.ClaimStatus = "Pending";

                model.DocumentsUploaded = uploadFile != null && uploadFile.Length > 0
                    ? uploadFile.FileName : null;
                model.Lecturer = HttpContext.Session.GetString("UserName");
                model.claimid = nextClaimId++; 
                //claimsList.Count > 0 ? claimsList.Max(c => c.claimid) + 1; 
                //nextClaimId++;

                claimsList.Add(model);


             TempData["UploadMessage"] = model.DocumentsUploaded != null
                    ? $"File'{uploadFile.FileName}' uploaded successfully." :"No file uploaded";
                }
                //then we add to a list 
          return View(model);
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



        //We then create dashboards for the other roles 
        public IActionResult CoordinatorDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Coordinator")
                return RedirectToAction("Dashboard");


            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult ManagerDashboard() {

            var role = HttpContext.Session.GetString("UserRole");

            if (role!= "Program Manager")
                return RedirectToAction("Dashboard");


            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var verifiedClaims = claimsList.Where(c=>c.ClaimStatus == "Verified").ToList();
            return View(verifiedClaims);
        }

        public IActionResult ApproveClaim(int index)//this is for the coordinator
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Coordinator")
                return RedirectToAction("Index");

            if (index >= 0 && index < claimsList.Count)
            {
                claimsList[index].ClaimStatus = "Verified";
                TempData["Message"] = $"Claim {index + 1} Verified";
            }
            return RedirectToAction("CoordinatorDashboard");
        }

        public IActionResult RejectClaim(int index)//this is for the coordinator
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role !="Program Coordinator")
                return RedirectToAction("Index");

            if (index >= 0 && index < claimsList.Count)
            {
                claimsList[index].ClaimStatus = "Denied";
                TempData["Message"] = $"Claim {index + 1} Denied";
            }
            return RedirectToAction("CoordinatorDashboard");
        }

        public IActionResult ApproveClaimManager(int index)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Manager")
                return RedirectToAction("Index");

            // Check for valid index and that the claim is Verified
            if (index >= 0 && index < claimsList.Count && claimsList[index].ClaimStatus == "Verified")
            {
                claimsList[index].ClaimStatus = "Approved";
                TempData["Message"] = $"Claim {index + 1} Approved successfully.";
            }
            else
            {
                TempData["Message"] = "Invalid claim selection or claim is not verified.";
            }

            return RedirectToAction("ManagerDashboard");
        }

        public IActionResult RejectClaimManager(int index)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Manager")
                return RedirectToAction("Index");

            // Check for valid index and that the claim is Verified
            if (index >= 0 && index < claimsList.Count && claimsList[index].ClaimStatus == "Verified")
            {
                claimsList[index].ClaimStatus = "Denied";
                TempData["Message"] = $"Claim {index + 1} Denied successfully.";
            }
            else
            {
                TempData["Message"] = "Invalid claim selection or claim is not verified.";
            }

            return RedirectToAction("ManagerDashboard");
        }



        public IActionResult ViewClaims()//for the coordinator to be able to t=see the claims made by lecturer
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Coordinator")
                return RedirectToAction("Index");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View("ViewClaims", claimsList);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
