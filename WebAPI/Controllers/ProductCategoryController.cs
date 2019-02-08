using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using Model.Models;
using Service;
using WebAPI.Infrastructure.Core;
using WebAPI.Infrastructure.Extensions;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/productCategory")]
    [Authorize]
    public class ProductCategoryController : ApiControllerBase
    {
        #region Initialize

        private IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService)
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
        }

        #endregion Initialize

        [Route("getallfilter")]
        [HttpGet]
        public IHttpActionResult GetAll(string filter)
        {

            var model = _productCategoryService.GetAll();

            var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

            return Ok(responseData);
        }
        [Route("getallhierachy")]
        [HttpGet]
        public IHttpActionResult GetAllHierachy()
        {
            return Ok(GetCategoryViewModel());
        }
        [Route("detail/{id:int}")]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            var model = _productCategoryService.GetById(id);

            var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(model);
            return Ok(responseData);
        }

        [Route("getall")]
        [HttpGet]
        public IHttpActionResult GetAll(string keyword, int page, int pageSize = 20)
        {
            int totalRow = 0;
            var model = _productCategoryService.GetAll(keyword);

            totalRow = model.Count();
            var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

            var responseData = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(query);

            var paginationSet = new PaginationSet<ProductCategoryViewModel>()
            {
                Items = responseData,
                PageIndex = page,
                TotalRows = totalRow,
                PageSize = pageSize
            };
            return Ok(paginationSet);
        }

        [Route("add")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Create(ProductCategoryViewModel productCategoryVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var newProductCategory = new ProductCategory();
                newProductCategory.UpdateProductCategory(productCategoryVm);
                newProductCategory.CreatedDate = DateTime.Now;
                _productCategoryService.Add(newProductCategory);
                _productCategoryService.Save();

                var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(newProductCategory);
                return Ok(responseData);
            }

        }

        [Route("update")]
        [HttpPut]
        public IHttpActionResult Update(ProductCategoryViewModel productCategoryVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                if (productCategoryVm.ID == productCategoryVm.ParentID)
                {
                    return BadRequest();
                }
                else
                {
                    var dbProductCategory = _productCategoryService.GetById(productCategoryVm.ID);

                    dbProductCategory.UpdateProductCategory(productCategoryVm);
                    dbProductCategory.UpdatedDate = DateTime.Now;

                    _productCategoryService.Update(dbProductCategory);
                    try
                    {
                        _productCategoryService.Save();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                    var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(dbProductCategory);
                    return Ok(responseData);
                }

            }
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var oldProductCategory = _productCategoryService.Delete(id);
                _productCategoryService.Save();

                var responseData = Mapper.Map<ProductCategory, ProductCategoryViewModel>(oldProductCategory);
                return Ok(responseData);
            }
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public IHttpActionResult DeleteMulti(string checkedProductCategories)
        {
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                else
                {
                    var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedProductCategories);
                    foreach (var item in listProductCategory)
                    {
                        _productCategoryService.Delete(item);
                    }

                    _productCategoryService.Save();
                    return Ok(listProductCategory.Count);
                }

            }
        }

        private List<ProductCategoryViewModel> GetCategoryViewModel(long? selectedParent = null)
        {
            List<ProductCategoryViewModel> items = new List<ProductCategoryViewModel>();

            //get all of them from DB
            var allCategorys = _productCategoryService.GetAll();
            //get parent categories
            IEnumerable<ProductCategory> parentCategorys = allCategorys.Where(c => c.ParentID == null);

            foreach (var cat in parentCategorys)
            {
                //add the parent category to the item list
                items.Add(new ProductCategoryViewModel
                {
                    ID = cat.ID,
                    Name = cat.Name,
                    Status = cat.Status,
                    CreatedDate = cat.CreatedDate
                });
                //now get all its children (separate Category in case you need recursion)
                GetSubTree(allCategorys.ToList(), cat, items);
            }
            return items;
        }
        private void GetSubTree(IList<ProductCategory> allCats, ProductCategory parent, IList<ProductCategoryViewModel> items)
        {
            var subCats = allCats.Where(c => c.ParentID == parent.ID);
            foreach (var cat in subCats)
            {
                //add this category
                items.Add(new ProductCategoryViewModel
                {
                    ID = cat.ID,
                    Name = parent.Name + " >> " + cat.Name,
                    Status = cat.Status,
                    CreatedDate = cat.CreatedDate
                });
                //recursive call in case your have a hierarchy more than 1 level deep
                GetSubTree(allCats, cat, items);
            }
        }
    }
}