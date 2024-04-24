using CantoLiterário___projeto_LivroOnline.Models;
using CantoLiterário___projeto_LivroOnline.Models.DTOs;
using CantoLiterário___projeto_LivroOnline.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CantoLiterário___projeto_LivroOnline.Controllers
{
    public class AccountController : Controller
    {
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        ICreateImageService _imageGenerator;
        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager,ICreateImageService imageGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageGenerator = imageGenerator;
        }
        [HttpGet]
        public IActionResult Register()
        {

            if (!_signInManager.IsSignedIn(User))
            {
                return View();
            }else return RedirectToAction("Index","Home");
            
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            
            if (ModelState.IsValid)
            {
                string urlImg = model.UserName + ".png";
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    ImgUrl = urlImg,
                    BackgroundUrl = "defaultBackground.jpg"
                };

                _imageGenerator.GenerateImage(urlImg);

                var result = await _userManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "home");
                }

                // Se houver erros então inclui no ModelState
                // que será exibido pela tag helper summary na validação
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return View();
            }
            else return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Login(LoginDTO model)
        {

            if (ModelState.IsValid)
            {


                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,model.RememberMe,false);


                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "home");
                }

                ModelState.AddModelError(string.Empty, "Login Inválido");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "home");
        }
    }
}
