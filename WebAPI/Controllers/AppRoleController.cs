using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common.Exceptions;
using Model.Models;
using Service;
using WebAPI.App_Start;
using WebAPI.Infrastructure.Core;
using WebAPI.Infrastructure.Extensions;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/appRole")]
    [Authorize]
    public class AppRoleController : ApiControllerBase
    {
        public AppRoleController(IErrorService errorService) : base(errorService)
        {
        }

        [Route("getlistpaging")]
        [HttpGet]
        public IHttpActionResult GetListPaging(int page, int pageSize, string filter = null)
        {
            int totalRow = 0;
            var query = AppRoleManager.Roles;
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Description.Contains(filter));
            totalRow = query.Count();

            var model = query.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);

            IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<AspNetRole>, IEnumerable<ApplicationRoleViewModel>>(model);

            PaginationSet<ApplicationRoleViewModel> pagedSet = new PaginationSet<ApplicationRoleViewModel>()
            {
                PageIndex = page,
                TotalRows = totalRow,
                PageSize = pageSize,
                Items = modelVm
            };

            return Ok(pagedSet);
        }

        [Route("getlistall")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
                var model = AppRoleManager.Roles.ToList();
                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<AspNetRole>, IEnumerable<ApplicationRoleViewModel>>(model);
                return Ok(modelVm);
        }

        [Route("detail/{id}")]
        [HttpGet]
        public IHttpActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            AspNetRole appRole = AppRoleManager.FindById(id);
            if (appRole == null)
            {
                return Ok(appRole);
            }
            return Ok(appRole);
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult Create(ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppRole = new AspNetRole();
                newAppRole.UpdateApplicationRole(applicationRoleViewModel);
                try
                {
                    AppRoleManager.Create(newAppRole);
                    return Ok(applicationRoleViewModel);
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

        [HttpPut]
        [Route("update")]
        public IHttpActionResult Update(ApplicationRoleViewModel applicationRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var appRole = AppRoleManager.FindById(applicationRoleViewModel.Id);
                try
                {
                    appRole.UpdateApplicationRole(applicationRoleViewModel, "update");
                    AppRoleManager.Update(appRole);
                    return Ok(appRole);
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
        public IHttpActionResult Delete(string id)
        {
            var appRole = AppRoleManager.FindById(id);

            AppRoleManager.Delete(appRole);
            return Ok(id);
        }
    }
}