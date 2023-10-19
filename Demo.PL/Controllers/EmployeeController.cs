using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        //private readonly IDepartmentRepository _departmentRepository;
        //private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IUnitOfWork unitOfWork/*IEmployeeRepository employeeRepository*//*, IDepartmentRepository departmentRepository*/, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            _mapper = mapper;
            //_departmentRepository = departmentRepository;
        }
        public async Task<IActionResult> Index(string searchInput)
        {
            var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(searchInput))
                employees = await _unitOfWork.EmployeeRepository.GetAll();
            //ViewData["Message"] = "Hello ViewData";
            //ViewBag.Message = "Hello ViewBag";
            else
                employees = _unitOfWork.EmployeeRepository.SearchByName(searchInput.ToLower());


            var mappedEmps = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);

            return View(mappedEmps);

        }

        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Manual Mapping
                //var mappedemp = new Employee()
                //{
                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,
                //    Email = model.Email,
                //    HireDate = model.HireDate,
                //    IsActive = model.IsActive,
                //    Salary = model.Salary,
                //    PhoneNumber = model.PhoneNumber,
                //};
                //Employee mappedEmp = (Employee) model;

                model.ImageName = await DocumentSettings.UploadFile(model.Image, "images");

                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(model);
                _unitOfWork.EmployeeRepository.Add(mappedEmp);
                int count = await _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created Successfully";
                }
                else
                {
                    TempData["Message"] = "An Error Has Ouccerd, Department NOT Created :(";

                }
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

                var employee = await _unitOfWork.EmployeeRepository.Get(id.Value);

                var MappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);

                if (employee is null)
                    return NotFound(); // 404
                //employee.Name = "HR #";
                return View(viewName, MappedEmp);

            }
            catch (Exception ex)
            {

                //_logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }

        }


        // Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewData["Departments"] = _departmentRepository.GetAll();

            ///if (id is null)
            ///    return BadRequest();
            ///var employee = _employeeRepository.Get(id.Value);
            ///if (employee is null)
            ///    return NotFound();
            ///return View(employee);
            ///
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if(model.ImageName is not null)
                        DocumentSettings.DeleteFile(model.ImageName, "images");
                    model.ImageName = await DocumentSettings.UploadFile(model.Image, "images");
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(model);
                    _unitOfWork.EmployeeRepository.Update(mappedEmp);
                    var count = await _unitOfWork.Complete();

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
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(model);

                    _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                    var count = await _unitOfWork.Complete();
                    if (count > 0)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images");
                    }
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
