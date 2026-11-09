using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InterviewsControl.Models.ViewModels;

namespace InterviewsControl.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;

        
        public AccountController(IConfiguration config)
        {
            _config = config;
        }
        // 顯示登入畫面
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // 處理登入請求
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                string validUser = _config["AdminCredentials:Username"] ?? Guid.NewGuid().ToString();
                string validPass = _config["AdminCredentials:Password"] ?? Guid.NewGuid().ToString();
                if (model.Username == validUser && model.Password == validPass)
                {
                    // 1. 建立宣告 (Claims) - 也就是這張身分證上要記載的資訊
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim("FullName", "吳宗鴻 (系統管理員)"),
                        new Claim(ClaimTypes.Role, "Administrator")
                    };

                    // 2. 建立身分識別
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 3. 登入並發放 Cookie
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    // 4. 登入成功後，導回原本想去的頁面，或是回首頁
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "帳號或密碼錯誤。");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // 登出
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}