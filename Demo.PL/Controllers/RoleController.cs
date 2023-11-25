using AutoMapper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper  _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager  ,IMapper mapper)
        {
           _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {
            if(string.IsNullOrEmpty(name))
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
                if(role is not null)
                {
                    var mappedRole = new RoleViewModel()
                    {
                        Id = role.Id,
                        RoleName = role.Name
                    };
                    return View(new List<RoleViewModel> { mappedRole });

                }
                return View(Enumerable.Empty<RoleViewModel>()); 
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (RoleViewModel roleVm)
        {
            if(ModelState.IsValid)
            {
              var mappedRole = _mapper.Map<RoleViewModel ,IdentityRole>(roleVm); 
                await _roleManager.CreateAsync(mappedRole);
                return RedirectToAction(nameof(Index));
            }
            return View(roleVm);
        }
        public async Task<IActionResult> Details(string id ,string ViewName= "Details")
        {
            if(id is null)
            {
                return BadRequest();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                return NotFound();
            }
            var mappedRole = new RoleViewModel ()
            {
                Id =role.Id,
                RoleName = role.Name
            };
            return View(ViewName, mappedRole); 

        }

        public async  Task<IActionResult> Edit(string id)
        {
            return  await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit ([FromRoute]string id ,RoleViewModel UpdatedRole )
        {
             if( id != UpdatedRole.Id)
            
                return BadRequest();
             if(ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync (id);
                    
                    role.Name = UpdatedRole.RoleName;

                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction("Index");
                }
                catch (Exception ex )
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    throw;
                }
            }

             return View(UpdatedRole);
               
        }

        public async Task<IActionResult>Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id ,RoleViewModel deletedRole)
        {
            if( id != deletedRole.Id)
                return BadRequest();

            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);
                return RedirectToAction("Index");

            }
            catch (Exception ex )
            {
                ModelState.AddModelError (string.Empty, ex.Message);    
                return View(deletedRole);
            }
            
        }
    }

}
