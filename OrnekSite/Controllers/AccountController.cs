using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using OrnekSite.Entity;
using OrnekSite.Identity;
using OrnekSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrnekSite.Controllers
{
    public class AccountController : Controller
    {
        DataContext db = new DataContext();
        //UserManager kullanıcı yönetimini sağlayan sınıftır.
        private UserManager<ApplicationUser> UserManager;
        //RoleManager kullanıcıların rölünü belirleyen sınıftır.
        private RoleManager<ApplicationRole> RoleManager;
        public AccountController()
        {
            var userStore = new UserStore<ApplicationUser>(new IdentityDataContext());
            UserManager = new UserManager<ApplicationUser>(userStore);
            //Kulanıcılar tanımladık ve yönetiyoruz

            var roleStore = new RoleStore<ApplicationRole>(new IdentityDataContext());
            RoleManager = new RoleManager<ApplicationRole>(roleStore);
            //Roller tanımlandı.
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid) 
            {
                var result = UserManager.ChangePassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                return View("Update");
            }
            return View(model);
        }


        public PartialViewResult UserCount() 
        {
            var u = UserManager.Users;
            return PartialView(u);

        }
        public ActionResult UserList()
        {
            var u = UserManager.Users;
            return View(u);

        }

        // GET: Account
        //Kullanıcının bilgilerini gncellemesi için
        public ActionResult UserProfil()
        {
            var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();//Bize giriş yapan kullanıcının idsini verir.
            var user = UserManager.FindById(id);//Kullanıcı bulundu.
            var data = new UserProfile()
            {
                id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.UserName
            };
            return View(data);
        }
        [HttpPost]
        public ActionResult UserProfil(UserProfile model)
        {
            
            var user = UserManager.FindById(model.id);
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.UserName = model.Username;
            UserManager.Update(user);
            return View("Update");

        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();//Cookieler çıkartılır
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public ActionResult Login(Login model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //Kullanıcı adını ve şifresini doğru girdiyse bize bir kullanıcı bulacaktır.
                var user = UserManager.Find(model.Username, model.Password);  
                if(user!=null)//Kullanıcı varsa 
                   {
                    var authManager = HttpContext.GetOwinContext().Authentication;//metot çağrılarak authManager oluşturulu.
                    var IdentityClaims = UserManager.CreateIdentity(user, "ApplicationCookie");
                    //UserManager nesnesi kullanılarak bu user için bir claims nesnesi oluşturulur.
                    var authProperties = new AuthenticationProperties();
                    authProperties.IsPersistent = model.RememberMe;
                    authManager.SignIn(authProperties, IdentityClaims);
                    if(!string.IsNullOrEmpty(returnUrl))//Kullanıcı giriş yapmadan izni olmayan sayfalara girmek isterse giriş sayfasına yönlendiriliyor.
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");//Giriş yaptıktan sonra
                   }
                else
                {
                    ModelState.AddModelError("LoginUserError", "Böyle bir kulllanıcı yok");
                }
            }
            return View(model);
        }


        public ActionResult Register()
        {
            return View();
        }

        //register metodunu post versiyonunda identity userdan türetilen application user sınıfından bir moddel oluşturuluyor.
        //Usermanager nesnesi kullanılarak kullanıcı oluşturuldu.Başarılı olursa kullanıcıya her rol veriliyor.
        //Sisteme kaydolan her kullanıcı user tipiinde olacak.
        //Sistemdeki diğer rol admin rolüdür.
        [HttpPost]//Viewden gelen veriler bu methodun nesnesinde tutulacak
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)//Kullanıcı zorunlu alanları doğru doldurmuşsa oluşturulsun.
            {
                var user = new ApplicationUser();//Yeni kullanıcı oluşturulsun
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Email = model.Email;
                user.UserName = model.UserName;
                var result = UserManager.Create(user, model.Passsword);
                if (result.Succeeded)
                {
                    if (RoleManager.RoleExists("user"))//Rol Kontrolü yapıyor.
                    {
                        UserManager.AddToRole(user.Id, "user");
                    }
                    return RedirectToAction("Login", "Account");

                }
                else 
                {
                    ModelState.AddModelError("RegisterUserError", "Kullanıcı Oluşturma Hatası");
                }


            }
            return View(model);
        }
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var orders = db.Orders.Where(i => i.UserName == username).Select(i => new UserOrder
            {
                Id = i.Id,
                OrderNumber = i.OrderNumber,
                OrderState = i.OrderState,
                OrderDate = i.OrderDate,
                Total = i.Total

            }).OrderByDescending(i => i.OrderState).ToList();
            return View(orders);
        }
        public ActionResult Details(int id)
        {
            var model = db.Orders.Where(i => i.Id == id).Select(i => new OrderDetails()
            {
                OrderId = i.Id,
                OrderNumber = i.OrderNumber,
                Total = i.Total,
                OrderDate = i.OrderDate,
                OrderState = i.OrderState,
                Adres = i.Adres,
                Sehir = i.Sehir,
                Semt = i.Semt,
                Mahalle = i.Mahalle,
                PostaKodu = i.PostaKodu,
                OrderLines = i.OrderLines.Select(x => new OrderDetails.OrderLineModel()
                {
                    ProductId = x.ProductId,
                    Image = x.Product.Image,
                    ProductName = x.Product.Name,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            }).FirstOrDefault();
            return View(model);
        }
    }
}