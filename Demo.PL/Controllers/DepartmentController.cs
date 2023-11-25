using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    //Inheritance : DepartmentController is a controller
    //Aggerhation : DepartmentController has a DepartmentRepository
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfwork _unitOfwork;

        public DepartmentController(IMapper mapper , IUnitOfwork unitOfwork)//Ask CLR For Creation Object from Class Implmenting
        {
            _mapper = mapper;
            _unitOfwork = unitOfwork;
        }
        public async  Task<IActionResult> Index()
        {
            var departments = await _unitOfwork.DepartmentRepository.GetAll();
            var mappeddepartments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappeddepartments);
        }

        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if(ModelState.IsValid)
            {
                var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfwork.DepartmentRepository.Add(mappedDepartment);
                 await _unitOfwork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }

        public async Task<IActionResult> Details(int? id , string ViewName ="Details") 
        {
            if (id is null)
                return BadRequest();
            var department =await  _unitOfwork.DepartmentRepository.Get(id.Value);
            var mappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);

            if (department is null)
                return NotFound();
            return View(ViewName, mappedDepartment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await  Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id ,DepartmentViewModel departmentVM) 
        {
            if(id != departmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfwork.DepartmentRepository.Update(mappedDepartment);
                   await _unitOfwork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }

        // Modal Bootstrap
        public async Task<IActionResult> Delete(int? id)
        {
            var department =await  _unitOfwork.DepartmentRepository.Get(id.Value);
            _unitOfwork.DepartmentRepository.Delete(department);
           await  _unitOfwork.Complete();
            return RedirectToAction("Index");
        }


        //public IActionResult Delete(int? id) 
        //{
        //    return Details(id, "Delete");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Delete([FromRoute] int id , Department department)
        //{
        //    if(id !=department.Id)
        //        return BadRequest();
        //    try
        //    {
        //        _departmentRepository.Delete(department);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, ex.Message);
        //        return View(department);
        //    }
        //    }
        }
    }
