using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Common.Exceptions;
using Model.Models;
using Service;
using WebAPI.App_Start;
using WebAPI.Infrastructure.Core;
using WebAPI.Infrastructure.Extensions;
using WebAPI.Models;
using WebAPI.Providers;

namespace WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/appUser")]
    public class AppUserController : ApiControllerBase
    {

        public AppUserController(IErrorService errorService)
            : base(errorService)
        {
        }

        [Route("getlistpaging")]
        [HttpGet]
        [Permission(Action = "Read", Function = "USER")]
        public IHttpActionResult GetListPaging(int page, int pageSize, string filter = null)
        {
            int totalRow = 0;
            var model = AppUserManager.Users;
            if (!string.IsNullOrWhiteSpace(filter))
                model = model.Where(x => x.UserName.Contains(filter) || x.Name.Contains(filter));

            totalRow = model.Count();

            var data = model.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);
            IEnumerable<AppUserViewModel> modelVm = Mapper.Map<IEnumerable<AspNetUser>, IEnumerable<AppUserViewModel>>(data);

            PaginationSet<AppUserViewModel> pagedSet = new PaginationSet<AppUserViewModel>()
            {
                PageIndex = page,
                PageSize = pageSize,
                TotalRows = totalRow,
                Items = modelVm,
            };

            return Ok(pagedSet);
        }

        [Route("detail/{id}")]
        [HttpGet]
        [Permission(Action = "Read", Function = "USER")]
        //[Authorize(Roles = "ViewUser")]
        public async Task<IHttpActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var user = await AppUserManager.FindByIdAsync(id);
            if (user == null)
            {
                return Ok(user);
            }
            else
            {
                var roles = await AppUserManager.GetRolesAsync(user.Id);
                var applicationUserViewModel = Mapper.Map<AspNetUser, AppUserViewModel>(user);
                applicationUserViewModel.Roles = roles;
                return Ok(applicationUserViewModel);
            }
        }

        [HttpPost]
        [Route("add")]
        //[Authorize(Roles = "AddUser")]
        [Permission(Action = "Create", Function = "USER")]
        public async Task<IHttpActionResult> Create(AppUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppUser = new AspNetUser();
                newAppUser.UpdateUser(applicationUserViewModel);
                try
                {
                    newAppUser.Id = Guid.NewGuid().ToString();
                    var result = await AppUserManager.CreateAsync(newAppUser, applicationUserViewModel.Password);
                    if (result.Succeeded)
                    {
                        var roles = applicationUserViewModel.Roles.ToArray();
                        await AppUserManager.AddToRolesAsync(newAppUser.Id, roles);

                        return Ok(applicationUserViewModel);
                    }
                    else
                        return BadRequest();
                }
                catch (NameDuplicatedException)
                {
                    return BadRequest();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("update")]
        //[Authorize(Roles = "UpdateUser")]
        [Permission(Action = "Update", Function = "USER")]
        public async Task<IHttpActionResult> Update(AppUserViewModel applicationUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var appUser = await AppUserManager.FindByIdAsync(applicationUserViewModel.Id);
                try
                {
                    appUser.UpdateUser(applicationUserViewModel);
                    var result = await AppUserManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        var userRoles = await AppUserManager.GetRolesAsync(appUser.Id);
                        var selectedRole = applicationUserViewModel.Roles.ToArray();

                        selectedRole = selectedRole ?? new string[] { };

                        await AppUserManager.AddToRolesAsync(appUser.Id, selectedRole.Except(userRoles).ToArray());
                        return Ok(applicationUserViewModel);
                    }
                    else
                        return BadRequest();
                }
                catch (NameDuplicatedException)
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("delete")]
        [Permission(Action = "Delete", Function = "USER")]
        //[Authorize(Roles ="DeleteUser")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var appUser = await AppUserManager.FindByIdAsync(id);
            var result = await AppUserManager.DeleteAsync(appUser);
            if (result.Succeeded)
                return Ok(id);
            else
                return Ok(string.Join(",", result.Errors));
        }
    }
}