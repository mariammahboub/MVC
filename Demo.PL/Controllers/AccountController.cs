using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManger;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManger  ,SignInManager<ApplicationUser> signInManager)
        {
			_userManger = userManger;
			_signInManager = signInManager;
		}
        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
		public async  Task<IActionResult> Register(RegisterViewModel model)
		{
            if(ModelState.IsValid)
            {
				var user = new ApplicationUser()
				{
					UserName = model.Email.Split("@")[0],
					Email = model.Email,
					IsAgree =model.IsAgree,
					FName = model.FName,
					LName = model.LName
				};
				var Result = await _userManger.CreateAsync(user,model.Password);
				if(Result.Succeeded)
				{

					return RedirectToAction(nameof(Login));	
				}
			
			 foreach(var error in Result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				
			}
           return View(model);

			
		}
		#endregion

		#region Login 

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}


		[HttpPost]
		public async  Task<IActionResult> Login(LoginViewModel model)
		{
			if(ModelState.IsValid)
			{
				var user=  await _userManger.FindByEmailAsync(model.Email);
				if(user is not null)
				{
					var flag = await _userManger.CheckPasswordAsync(user , model.Password);
					if(flag)
					{
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RemeberMe, false);
						if(result.Succeeded)
						{
							return RedirectToAction(nameof(HomeController.Index),"Home");	
						}
					}
				}

			}
			ModelState.AddModelError(string.Empty, "Invaild Login");
			return View(model);
		}

		#endregion

		#region Sign Out
		public  new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return View(nameof(Login));	
		}
		#endregion

		#region Forget Password
		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
		{
			if(ModelState.IsValid)
			{
				var user = await _userManger.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var token = await _userManger.GeneratePasswordResetTokenAsync(user);//Unique fot this user for one Time 
					var ResetPasswordUrl = Url.Action("ResetPassword" ,"Account", new {email =model.Email, token = token });//Generate UrL Based on Action Passed 

					//http://localhost:45473/Account/ResetPassword?email=nada@gmail.com&token
					var Email = new Email()
				   {
					   Subject = "Reset Your Password ",
					   Recipients = model.Email,
					   Body = "ResetPasswordUrl"
					};
					EmailSettings.SendEmail(Email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				ModelState.AddModelError(string.Empty, "User not Exist ");
				
			}
			return View(model);
		}

		public IActionResult CheckYourInbox()
		{
			return View();
		}
		#endregion

		#region ResetPassword

		#endregion
	}
}
