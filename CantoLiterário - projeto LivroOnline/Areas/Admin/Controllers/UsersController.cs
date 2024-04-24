using CantoLiterário___projeto_LivroOnline.Context;
using CantoLiterário___projeto_LivroOnline.Models;
using CantoLiterário___projeto_LivroOnline.Models.DTOs;
using CantoLiterário___projeto_LivroOnline.Services.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CantoLiterário___projeto_LivroOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        public UserManager<User> _user;
        public AppDbContext _dbContext;
        public RoleManager<IdentityRole> _role{ get; set; }
        public IMapper _mapper { get; set; }


        public UsersController(UserManager<User> user, AppDbContext dbContext, RoleManager<IdentityRole> role, IMapper mapper)
        {
            _user = user;
            _dbContext = dbContext;
            _role = role;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var userList = _dbContext.Users.Select(u => _mapper.Map<UserDTO>(u)).ToList();
            return View(userList);
        }

        public async Task Delete(string userId)
        {
            var user = await _user.FindByIdAsync(userId);
            var list = _dbContext.Comments.Where(c => c.AuthorId == user.Id).ToList();
            var listFollowers = _dbContext.Users
                .Where(c => c.Id == userId)
                    .Include(u => u.Followers).FirstOrDefault();
            listFollowers?.Followers?.Clear();
            _dbContext.SaveChanges();

            if (list.Count > 0)
            {
                
                _dbContext.RemoveRange(list);
            }
            await _user.DeleteAsync(user);
        }

        public async Task<IActionResult> UserRoles(string userId)
        {
            var user = await _user.FindByIdAsync(userId);
            return PartialView("_PermissionsContainerPartial", user);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(string userId, string RoleName)
        {
            var user = await _user.FindByIdAsync(userId);
            await _user.AddToRoleAsync(user, RoleName);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRoleToUser(string userId, string RoleName)
        {
            var user = await _user.FindByIdAsync(userId);

            await _user.RemoveFromRoleAsync(user, RoleName);
            return RedirectToAction("Index");
        }
    }
}
