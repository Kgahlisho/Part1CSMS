using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Part1ex.Models;
using System.Collections.Generic;
using Part1ex.Data;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;



namespace Part1ex.Controllers
{
    //create an im-memeory storage for incoming and new claims which is temporary
    public class HomeController : Controller
    {
       // private static List<Calculations> claimsList = new List<Calculations>();
        private readonly ApplicationDbContext _dbContext;

       // private static int nextClaimId;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        
        
        public async Task<IActionResult> Login(string Email, string Password)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                TempData["LoginError"] = "Please enter email and password ";
                return RedirectToAction("Index");
            }

            var user = await _dbContext.Roles.FirstOrDefaultAsync(u => u.Email == Email && u.Password == Password);

            if (user != null)
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
            //if the user is not found
            TempData["LoginError"] = "Invalid email or password";
            return RedirectToAction("Index");


        }
        /*
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
        */
      [HttpGet]
        public IActionResult Register()
            {
                return View();
            }

       [HttpPost]
        public async Task<IActionResult> Register(Rol model)
        {
            if (!ModelState.IsValid)
            { //can be able to redirect to register 

                TempData["RegisterError"] = "All fields are required.";
                return View(model);
            }

            if (await _dbContext.Roles.AnyAsync(r => r.Email == model.Email))
            {
                TempData["RegisterError"] = "Email is already registered";
                return View(model);
            }

            await _dbContext.Roles.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registration Successful. Login";
            return RedirectToAction("Index");
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

        // private static int nextClaimId = 1;

        [HttpGet]
        public IActionResult Claims()
        {
            var model = new Calculations();

            if (TempData.ContainsKey("TotalAmount"))
            {
                model.TotalAmount = Convert.ToDecimal(TempData["TotalAmount"]);
            }

            if (TempData.ContainsKey("UploadMessage"))
            {
                ViewBag.UploadMessage = TempData["UploadMessage"];
            }

            if (TempData.ContainsKey("ClaimError"))
            {
                ViewBag.ClaimError = TempData["ClaimError"];
            }

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Claims(Calculations model, IFormFile? uploadFile)
        {
            if (model == null || model.HoursWorked <= 0 || model.HourlyRate <=0)
            //|| !ModelState.IsValid || model.HoursWorked <= 0 || model.HourlyRate <= 0)
            {
                TempData["ClaimError"] = "Please ensure all fields are valid and positive values are entered.";
                return View(new Calculations());

            }


            model.TotalAmount = model.HoursWorked * model.HourlyRate;
            model.ClaimDate = DateTime.Now;
            model.ClaimStatus = "Pending";
            model.DocumentsUploaded = uploadFile != null && uploadFile.Length > 0
                ? uploadFile.FileName : null;
            model.Lecturer = HttpContext.Session.GetString("UserName") ?? "Unkown";

            ModelState.Clear();
            if (!TryValidateModel(model))
            {
                TempData["ClaimError"] = "Please ensure all fields are valid.";
                return View(model);
            }

            try
            {

                await _dbContext.Claims.AddAsync(model);
                await _dbContext.SaveChangesAsync();



                TempData["UploadMessage"] = model.DocumentsUploaded != null
                  ? $"File'{uploadFile.FileName}' uploaded successfully." : "No file uploaded";

                TempData["TotalAmount"] = model.TotalAmount;

                return RedirectToAction("Claims");
            }
            catch (Exception ex)
            {
                TempData["ClaimError"] = $"Error saving claim:{ex.Message}";
                return View(model);
            }

        }
         //      
           // }
           

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

        public async Task<IActionResult> Claims_Dashboard()//now it feathces from EF 
        {
            var lecturerName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(lecturerName)) return RedirectToAction("Index");

            
            var claims = await _dbContext.Claims.Where(c => c.Lecturer == lecturerName).ToListAsync();
            return View(claims);
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
                TempData["UploadMessage"] = "No file Selected.";
            }
            return  RedirectToAction("Claims");
        }

        


        //We then create dashboards for the other roles 
        public async Task<IActionResult> CoordinatorDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Coordinator")
                return RedirectToAction("Dashboard");


            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            var claims = await _dbContext.Claims.ToListAsync();
            return View("ViewClaims" , claims);
        }

        public async Task<IActionResult> ManagerDashboard() {

            var role = HttpContext.Session.GetString("UserRole");
            if (role!= "Program Manager")
                return RedirectToAction("Dashboard");


            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            var verifiedClaims = await _dbContext.Claims
                .Where(c => c.ClaimStatus == "Verified" || c.ClaimStatus == "Approved").ToListAsync();
            
            return View(verifiedClaims);
        }




        public async Task<IActionResult> ApproveClaim(int claimid)//this is for the coordinator
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Coordinator")
                return RedirectToAction("Index");

            var claim = await _dbContext.Claims.FindAsync(claimid);
            if (claim != null)
            {
                claim.ClaimStatus = "Verified";
                await _dbContext.SaveChangesAsync();
                TempData["Message"] = $"Claim{claim.claimid} has been Verified.";
            }
            else
            {
                TempData["Message"] = "Claim not found";
            }
                return RedirectToAction("ViewClaims");
        }


        public async Task<IActionResult> RejectClaim(int claimid)//this is for the coordinator
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Coordinator")
                return RedirectToAction("Index");

            var claim = await _dbContext.Claims.FindAsync(claimid);
            if (claim != null)
            {
                claim.ClaimStatus = "Denied";
                await _dbContext.SaveChangesAsync();
                TempData["Message"] = $"Claim {claim.claimid} Denied.";
            }
            else
            {
                TempData["Message"] = "Claim not found.";
            }
                return RedirectToAction("CoordinatorDashboard");
        }


        public async Task<IActionResult> ApproveClaimManager(int claimid)//this is used by the Manager
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Manager")
                return RedirectToAction("Index");

            // Check for valid index and that the claim is Verified
            var claim = await _dbContext.Claims.FindAsync(claimid);
            if (claim == null)
            {
                TempData["Message"] = "Claim not found.";
            } 
            else if (claim.ClaimStatus == "Verified")
            {
                claim.ClaimStatus = "Approved";
                await _dbContext.SaveChangesAsync();
                TempData["Message"] = $"Claim {claim.claimid} Approved successfully.";

            }
            else
            {
                TempData["Message"] = "Invalid claim selection or claim not verified yet.";
            }
            return RedirectToAction("ManagerDashboard");
        }

        public async Task<IActionResult> RejectClaimManager(int claimid)//this is used by the Manager
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Manager")
                return RedirectToAction("Index");

            // Check for valid index and that the claim is Verified
            var claim = await _dbContext.Claims.FindAsync(claimid);
            if (claim != null && claim.ClaimStatus == "Verified")
            {
                claim.ClaimStatus = "Denied";
                await _dbContext.SaveChangesAsync();
                TempData["Message"] = $"Claim {claim.claimid} Denied successfully.";
            }
            else
            {
                TempData["Message"] = "Invalid claim selection or claim is not verified.";
            }

            return RedirectToAction("ManagerDashboard");
        }



        public async Task<IActionResult> ViewClaims()//for the coordinator to be able to t=see the claims made by lecturer
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Program Coordinator")
                return RedirectToAction("Index");


          //  ViewBag.UserName = HttpContext.Session.GetString("UserName");
            var claims = await _dbContext.Claims.ToListAsync();
            return View("ViewClaims",claims);
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
