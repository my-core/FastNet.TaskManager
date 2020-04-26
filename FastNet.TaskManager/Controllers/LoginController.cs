using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FastNet.TaskManager.Models;
using FastNet.TaskManager.Repository;

namespace FastNet.TaskManager.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        public QuartzRepository _yunYingRepository;
        public LoginController(ILogger<LoginController> logger, QuartzRepository yunYingRepository)
        {
            _logger = logger;
            _yunYingRepository = yunYingRepository;
        }

        public IActionResult Index()
        {
            HttpContext.SignOutAsync();
            return View();
        }
        [HttpPost]
        public IActionResult Index(string adminName, string password)
        {
            AjaxResult ajaxResult = new AjaxResult { IsOk = false, Msg = "用户名/密码不正确" };
            if (adminName == "admin" && password == "Dev@123456")
            {
                var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "admin"));

                var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                // 在上面注册AddAuthentication时，指定了默认的Scheme，在这里便可以不再指定Scheme。
                HttpContext.SignInAsync(claimsPrincipal);
                ajaxResult = new AjaxResult { IsOk = true };
            }
            return Json(ajaxResult);

        }
    }
}
