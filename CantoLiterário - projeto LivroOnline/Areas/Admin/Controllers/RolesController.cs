using CantoLiterário___projeto_LivroOnline.Areas.Admin.Models.DTOs;
using CantoLiterário___projeto_LivroOnline.Context;
using CantoLiterário___projeto_LivroOnline.Models;
using CantoLiterário___projeto_LivroOnline.Models.DTOs;
using CantoLiterário___projeto_LivroOnline.Services.Mapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CantoLiterário___projeto_LivroOnline.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
		public UserManager<User> _userManager { get; set; }
        public RoleManager<IdentityRole> _roleManager { get; set; }
        public AppDbContext _dbContext { get; set; }
        public IMapper _mapper { get; set; }



		public RolesController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, AppDbContext dbContext, IMapper mapper)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public IActionResult Index()
        {
            var list = _dbContext.Roles.Select(r =>  _mapper.Map<RoleDTO>(r)).ToList();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([Required][FromForm] String name)
        {
            await _roleManager.CreateAsync(new IdentityRole(name));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole([Required][FromForm] String id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
    }
}
