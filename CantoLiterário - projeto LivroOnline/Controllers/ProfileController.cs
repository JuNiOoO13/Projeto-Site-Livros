using CantoLiterário___projeto_LivroOnline.Context;
using CantoLiterário___projeto_LivroOnline.Models;
using CantoLiterário___projeto_LivroOnline.Models.DTOs;
using CantoLiterário___projeto_LivroOnline.Services.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CantoLiterário___projeto_LivroOnline.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        UserManager<User> _userManager { get; set; }
        SignInManager<User> _signInManger { get; set; }
        AppDbContext _context { get; set; }
        IMapper _mapper { get; set; }

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManger, AppDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _signInManger = signInManger;
            _context = context;
            _mapper = mapper;
        }

       
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            User? usuarioInfo = await _userManager.GetUserAsync(User);
            var user = _context.Users
                .Where(c => c.Id == usuarioInfo.Id)
                    .Include(c => c.Follows)
                        .Include(c => c.Books)
                            .Select(user => _mapper.Map<UserDTO>(user)).FirstOrDefault();

            return View(user);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserDTO user,IFormFile img,IFormFile imgBackground)
        {
            var userMain = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            ModelState.Remove("img");
            ModelState.Remove("imgBackground");
            if (img != null)
            {
                string extension = img.FileName.Substring(img.FileName.LastIndexOf('.'));
                if (img.ContentType.ToLower().StartsWith("image/"))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagens", "UsersImg", user.UserName + extension);
                    userMain.ImgUrl = user.UserName + extension;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    await _userManager.UpdateAsync(userMain);
                    return RedirectToAction("User");
                }
            }
            if (imgBackground != null)
            {
                string extension = imgBackground.FileName.Substring(imgBackground.FileName.LastIndexOf('.'));
                if (imgBackground.ContentType.ToLower().StartsWith("image/"))
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagens", "UsersBackground", user.UserName + extension);
                    userMain.BackgroundUrl = user.UserName + extension;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imgBackground.CopyToAsync(stream);
                    }
                    await _userManager.UpdateAsync(userMain);
                    return RedirectToAction("UserScreen", new { Id = userMain.Id });
                }
            }
            if (ModelState.IsValid)
            {
               
                userMain.PhoneNumber = user.PhoneNumber;
                userMain.Email = user.Email;
                userMain.UserName = user.UserName;
                userMain.Bio = user.Bio;
                await _userManager.UpdateAsync(userMain);
                return RedirectToAction("UserScreen", new { Id = userMain.Id });
            }
            
            return RedirectToAction("Edit");

        }

        [Route("Usuarios/{Id}")]
        public IActionResult UserScreen([FromRoute] string Id)
        {
            var user = _context.Users
                .Where(u => u.Id == Id)
                    .Include(b => b.Books)
                        .Include(a => a.Followers ).FirstOrDefault();

            UserDTO userDto;
            if (user != null){
                userDto = _mapper.Map<UserDTO>(user);
                return View(userDto);
            }
            return RedirectToAction("Error", "Home");


           
        }
        [Route("Follow/{Id}")]
        public async Task<IActionResult> Follow([FromRoute] string Id)
        {
            var userId = _userManager.GetUserId(User);
            User? user = await _userManager.FindByIdAsync(userId);
            User? userToFollow = await _userManager.FindByIdAsync(Id);
            if (userToFollow != null && user != null)
            {
                userToFollow.Followers = new List<User>() { user};
                await _userManager.UpdateAsync(userToFollow);
            }
            
           
            return RedirectToAction("UserScreen", new {Id = Id });
        }

        [Route("UnFollow/{Id}")]
        public async Task<IActionResult> UnFollow([FromRoute] string Id)
        {
            var userId = _userManager.GetUserId(User);
            User? user = await _userManager.FindByIdAsync(userId);
            User? userToFollow = _context.Users.Where(u => u.Id == Id).Include(f => f.Followers).FirstOrDefault();
            if (userToFollow != null && user != null)
            {
                userToFollow.Followers?.Remove(user);
                await _userManager.UpdateAsync(userToFollow);
            }


            return RedirectToAction("UserScreen", new { Id = Id });
        }
    }
}