using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfwork _unitOfwork;
        public EmployeeController(IMapper mapper, IUnitOfwork unitOfwork )
        {
            _mapper = mapper;
            _unitOfwork = unitOfwork;
        }

        public async Task<IActionResult> Index(string SearchInp)
        {
            var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(SearchInp))
                employees = await   _unitOfwork.EmployeeRepository.GetAll();
            else
                employees = _unitOfwork.EmployeeRepository.SearchByName(SearchInp);
            var mappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel >>(employees);
            return View(mappedEmployees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> Create(EmployeeViewModel EmployeeVM)
        {
            if (ModelState.IsValid)
            {
                EmployeeVM.ImageName = await DocumentSettings.UploadFile(EmployeeVM.Image, "images");
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);

                _unitOfwork.EmployeeRepository.Add(mappedEmployee);
                var count =  await _unitOfwork.Complete();
                if (count > 0)
                    TempData["MSG"] = "Employee Created Sucessfully:)";
                else
                    TempData["MSG"] = "An Error Has Occured , Employee Not Created :(";
                return RedirectToAction(nameof(Index));
            }
            return View(EmployeeVM);
        }

        public  async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var Employee = await _unitOfwork.EmployeeRepository.Get(id.Value);
            var mappedEmployee = _mapper.Map<Employee , EmployeeViewModel >(Employee);

            if (Employee is null)
                return NotFound();
            return View(ViewName, mappedEmployee);
        }

        public  async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit([FromRoute] int id, EmployeeViewModel EmployeeVM)
        {
            if (id != EmployeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmployee = _mapper.Map<EmployeeViewModel , Employee>(EmployeeVM);
                    await   _unitOfwork.DepartmentRepository.GetAll();
                    _unitOfwork.EmployeeRepository.Update(mappedEmployee);
                    await  _unitOfwork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(EmployeeVM);
        }

        public async  Task<IActionResult> Delete(int? id)
        {
            return  await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public   async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel EmployeeVM)
        {
            if (id != EmployeeVM.Id)
                return BadRequest();
            try
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVM);
                _unitOfwork.EmployeeRepository.Delete(mappedEmployee);
                var count =await  _unitOfwork.Complete();
                if(count > 0)
                {
                    DocumentSettings.DeleteFile(EmployeeVM.ImageName, "images");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(EmployeeVM);
            }
        }
    }
}
