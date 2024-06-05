using AutoMapper;
using DALayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLayer.Models;
using System.Buffers;

namespace PLayer.Controllers
{
    [Authorize (Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;


        public UserController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;

        }
        #region Index
        public async Task<IActionResult> Index(string SearchValue)
        {

            if (string.IsNullOrEmpty(SearchValue))
            {

                var Users = _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    PhoneNumber = U.PhoneNumber,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToList();
                return View(Users);
            }
            else
            {
                var User = _userManager.Users.Where(u => u.Email.ToLower().Contains(SearchValue.ToLower())).Select(U => new UserViewModel()

                {
                    Id = U.Id,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToList();

                return View(User);
            }

        }

        #endregion

        #region Details
        public async Task<IActionResult> Details(string Id, string viewName = "Details")
        {
            if (Id is null)
                return BadRequest();
            var User = await _userManager.FindByIdAsync(Id);
            if (User is null)
                return NotFound();
            var mappedUser = _mapper.Map<AppUser, UserViewModel>(User);
            return View(viewName, mappedUser);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Edit(string Id, string viewName)
        {
            return await Details(Id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string Id, UserViewModel model)
        {
            if (Id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)

            {
                try
                {
                    var User = await _userManager.FindByIdAsync(model.Id);

                    User.FirstName = model.FirstName;
                    User.LastName = model.LastName;
                    User.PhoneNumber = model.PhoneNumber;
                    await _userManager.UpdateAsync(User);

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
        public async Task<IActionResult> Delete([FromRoute] string Id, UserViewModel model)
        {
            if (Id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)

            {
                try
                {
                    var User = await _userManager.FindByIdAsync(model.Id);

                    await _userManager.DeleteAsync(User);
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

    }
}

