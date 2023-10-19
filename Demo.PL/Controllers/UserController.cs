using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                var users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FName = U.FirstName,
                    LName = U.LastName,
                    Email = U.Email,
                    PhoneNumber = U.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync();
                return View(users);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(email);
                var mappedUser = new UserViewModel()
                {
                    Id = user.Id,
                    FName = user.FirstName,
                    LName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(user).Result
                };

                return View(mappedUser);
            }


        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            try
            {
                if (id is null)
                    return BadRequest(); // 400

                var userFromDB = await _userManager.FindByIdAsync(id);
                if (userFromDB is null)
                    return NotFound(); // 404

                var user = new UserViewModel()
                {
                    PhoneNumber = userFromDB.PhoneNumber,
                    FName = userFromDB.FirstName,
                    LName = userFromDB.LastName,
                    Email = userFromDB.Email,
                    Id = userFromDB.Id
                };

                //user.Email = model.Email;
                //user.SecurityStamp = Guid.NewGuid().ToString();

                //employee.Name = "HR #";
                return View(viewName, user);

            }
            catch (Exception ex)
            {

                //_logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    //var MappedUser = _mapper.Map<UserViewModel, ApplicationUser>(model);
                    var user = await _userManager.FindByIdAsync(id);

                    user.PhoneNumber = model.PhoneNumber;
                    user.FirstName = model.FName;
                    user.LastName = model.LName;
                    user.Email = model.Email;
                    user.SecurityStamp = Guid.NewGuid().ToString();

                    await _userManager.UpdateAsync(user);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1. Log exception
                    // 2. friendly message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);

                    await _userManager.DeleteAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // 1. Log exception
                    // 2. friendly message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }


       

    }
}
