using AspMVC.Areas.Identity.Controllers;
using AspMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace AspMVC.Controllers
{
    public class TestController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public TestController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }
        [Route("/testemail")]
        public async Task<IActionResult> testemail()
        {
            await _emailSender.SendEmailAsync("notviet068@gmail.com",
                        "Xác nhận địa chỉ email",
                        @$"Bạn đã đăng ký tài khoản trên GameStore, 
                           hãy <a href=''>bấm vào đây</a> 
                           để kích hoạt tài khoản.");
            return View();
        }
    }
}
