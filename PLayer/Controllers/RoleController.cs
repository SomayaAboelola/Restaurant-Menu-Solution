using AutoMapper;
using DALayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLayer.Models;

namespace PLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;

        public RoleController(RoleManager<IdentityRole> roleManager,
                                UserManager<AppUser> userManager,
                                IMapper mapper,
                                ILogger<RoleController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            var Roles = await _roleManager.Roles.ToListAsync();
            var mappedRole = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RolesViewModel>>(Roles);
            return View(mappedRole);
        }

        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedRole = _mapper.Map<RolesViewModel, IdentityRole>(model);
                    var Role = await _roleManager.CreateAsync(mappedRole);

                    if (Role.Succeeded)
                        return RedirectToAction(nameof(Index));

                }
                catch (Exception error)
                {
                    _logger.LogError(error.Message);
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }
            return View(model);
        }

        #endregion

        #region Details
        public async Task<IActionResult> Details(string Id, string viewName = "Details")
        {
            if (Id is null)
                return BadRequest();
            var Role = await _roleManager.FindByIdAsync(Id);
            if (User is null)
                return NotFound();
            var mappedRole = _mapper.Map<IdentityRole, RolesViewModel>(Role);
            return View(viewName, mappedRole);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Edit(string Id, string viewName)
        {
            return await Details(Id, "Edit");

        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string Id, RolesViewModel model)
        {
            if (Id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)

            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);

                    role.Name = model.RoleName;

                    await _roleManager.UpdateAsync(role);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

            return View(model);
        }

        #endregion

        #region Delete
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            return await Details(Id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string Id, RolesViewModel model)
        {
            if (Id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)

            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);

                    await _roleManager.DeleteAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }

            return View();
        }

        #endregion

        #region AddOrRemoveUsers
        public async Task<IActionResult> AddOrRemoveUsers(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role is null)
                return NotFound();
            ViewBag.RoleId = RoleId;

            var userInRole = new List<UserInViewModel>();
            var Users = await _userManager.Users.ToListAsync();

            foreach (var user in Users)
            {
                var UserInRole = new UserInViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if (_userManager is not null)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                        UserInRole.IsSelected = true;
                    else
                        UserInRole.IsSelected = false;
                    userInRole.Add(UserInRole);
                }
            }
            return View(userInRole);

        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string RoleId, List<UserInViewModel> model)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role is null)
                return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var user in model)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        var IsInRole = await _userManager.IsInRoleAsync(appUser, role.Name);
                        if (user.IsSelected && !IsInRole)
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        else if (!user.IsSelected && IsInRole)
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                    }

                }
                return RedirectToAction(nameof(Edit), new { Id = RoleId, viewname = "Edit" });
            }
            return View(model);
        }


    
    #endregion
    } 
}