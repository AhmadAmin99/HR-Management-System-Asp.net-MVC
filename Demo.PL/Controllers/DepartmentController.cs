using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{

	[Authorize]
	public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;

        //private readonly IGenericRepository<Department> _departmentRepository;
        private readonly ILogger _logger;

        public DepartmentController(IMapper mapper, IUnitOfWork unitOfWork/*IDepartmentRepository departmentRepository*/, ILogger<DepartmentController> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            //_departmentRepository = departmentRepository;

        }
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAll();
            var mappedDept = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDept);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(model);
                _unitOfWork.DepartmentRepository.Add(mappedDept);
                int result = await _unitOfWork.Complete();
                if (result > 0)
                    return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            try
            {
                if (id is null)
                    return BadRequest(); // 400

                var department = await _unitOfWork.DepartmentRepository.Get(id.Value);
                var mappedDept = _mapper.Map<Department, DepartmentViewModel>(department);

                if (department is null)
                    return NotFound(); // 404
                //department.Name = "HR #";
                return View(viewName, mappedDept);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }


        // Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ///if (id is null)
            ///    return BadRequest();
            ///var department = _departmentRepository.Get(id.Value);
            ///if (department is null)
            ///    return NotFound();
            ///return View(department);
            ///
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, DepartmentViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDept = _mapper.Map<DepartmentViewModel, Department>(model);

                    _unitOfWork.DepartmentRepository.Update(mappedDept);
                    await _unitOfWork.Complete();
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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDept = _mapper.Map<DepartmentViewModel, Department>(model);

                    _unitOfWork.DepartmentRepository.Delete(mappedDept);
                    await _unitOfWork.Complete();

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