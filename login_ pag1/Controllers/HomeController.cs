using login__pag1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace login__pag1.Controllers
{
    public class HomeController : Controller
    {
        MyDBContext context  = new MyDBContext();
        public IActionResult Index()
        {
            /*if (HttpContext.Session.GetString("Emailid") == null)
            {
                // Redirect to the login page or take appropriate action
                return RedirectToAction("Login", "Home");
            }*/

            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user,string Email)
        {
            var myuser = context.Getuser().FirstOrDefault(x => x.Emailid == Email &&  x.Password == user.Password);
            if(myuser!= null)
            {
               
                return RedirectToAction("ValidateOTP");
            }

            else
            {
                ViewBag.Message = "login failed";
            }
            return View();
        }

       public JsonResult sendOtp(string Email)
        {
            string otp = GenerateOTP();
            context.sendEmailToClient(Email, otp);
            HttpContext.Session.SetString("otp", otp);
            HttpContext.Session.SetString("userSession", Email);
            return Json("true");
        }
       /* [HttpPost]
        public JsonResult sendOtp(string Email)
        {
            
            return Json("success");
        }*/
        public IActionResult ValidateOTP()
        {
            // Display the OTP validation view
            return View();
        }

        [HttpPost]
        public IActionResult ValidateOTP(string otp)
        {
            // Retrieve the stored OTP from session
            string storedOTP = HttpContext.Session.GetString("otp");

            if (otp == storedOTP)
            {
                // OTP is valid
                HttpContext.Session.SetString("otpVerified", "true");
                return RedirectToAction("Dashboard");
            }
            else
            {
                // OTP is invalid
                ViewBag.ErrorMessage = "Invalid OTP";
                return View();
            }
        }
        private string GenerateOTP()
        {
            Random rnd = new Random();
            int otp = rnd.Next(10000, 99999);
            return otp.ToString();
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("userSession") != null)
            {
                HttpContext.Session.Remove("userSession");
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("userSession")!= null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("userSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user , string ConfirmPassword , string Password)
        {
            if (true && ConfirmPassword==Password)
            {
                context.AddUser(user);
                ModelState.Clear();
                return RedirectToAction("Login");
            }
            return View();
        }

        public JsonResult List()
        {
            return Json(context.Getuser());
        }
        [HttpPost]
        public JsonResult Add([FromBody] User car)
        {
            return Json(context.AddUser(car));
        }
        [HttpPost]
        public JsonResult Update(User car)
        {
            return Json(context.UpdateUser(car));
        }
        [HttpGet]
        public JsonResult GetbyID(int id)
        {
            var car = context.Getuser().Find(x => x.Id.Equals(id));
            return Json(car);
        }
        [HttpPost]
        public JsonResult Delete(int ID)
        {
            return Json(context.DeleteUser(ID));
        }


    }
}

/*[HttpPost]
public async Task<IActionResult> Login(string Name, string Password)
{
    AccountDb accDB = new AccountDb();
    var user = accDB.getAllUser().FirstOrDefault(u => u.Name == Name && u.Password == Password);

    if (user != null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            // You can add more claims here based on your user object
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            // Add any properties you want
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        return RedirectToAction("Index", "Home");
    }
    else
    {
        // Add a ModelState error or redirect to the login page with a message
        ModelState.AddModelError(string.Empty, "Invalid username or password.");
        return View();
    }
}*/




    //return View();
//}