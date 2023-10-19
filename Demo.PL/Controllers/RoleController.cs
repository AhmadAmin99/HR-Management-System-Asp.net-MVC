using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();

                return View(roles);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(name);
                var mappedRole = new RoleViewModel()
                {
                    Id = role.Id,
                    RoleName = role.Name
                };

                return View(mappedRole);
            }


        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName,
                };
                await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            try
            {
                if (id is null)
                    return BadRequest(); // 400

                var roleFromDB = await _roleManager.FindByIdAsync(id);
                if (roleFromDB is null)
                    return NotFound(); // 404

                var role = new RoleViewModel()
                {
                   
                    Id = roleFromDB.Id,
                    RoleName = roleFromDB.Name
                    
                };

                //user.Email = model.Email;
                //user.SecurityStamp = Guid.NewGuid().ToString();

                //employee.Name = "HR #";
                return View(viewName, role);

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
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    //var MappedUser = _mapper.Map<UserViewModel, ApplicationUser>(model);
                    var role = await _roleManager.FindByIdAsync(id);

                    role.Name = model.RoleName;

                    await _roleManager.UpdateAsync(role);

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
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);

                    await _roleManager.DeleteAsync(role);
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
