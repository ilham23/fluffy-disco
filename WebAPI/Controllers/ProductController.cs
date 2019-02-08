using AutoMapper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Common;
using Model.Models;
using Service;
using WebAPI.Infrastructure.Core;
using WebAPI.Infrastructure.Extensions;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/product")]
    [Authorize]
    public class ProductController : ApiControllerBase
    {
        #region Initialize

        private IProductService _productService;

        public ProductController(IErrorService errorService, IProductService productService)
            : base(errorService)
        {
            this._productService = productService;
        }

        #endregion Initialize

        [Route("getallparents")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var model = _productService.GetAll();

            var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

            return Ok(responseData);
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            var model = _productService.GetById(id);

            var responseData = Mapper.Map<Product, ProductViewModel>(model);

            return Ok(responseData);
        }

        [Route("getall")]
        [HttpGet]
        public IHttpActionResult GetAll(int? categoryId, string keyword, int page, int pageSize = 20)
        {
            int totalRow = 0;
            var model = _productService.GetAll(categoryId, keyword);

            totalRow = model.Count();
            var query = model.OrderByDescending(x => x.CreatedDate).Skip(page - 1 * pageSize).Take(pageSize).ToList();

            var responseData = Mapper.Map<List<Product>, List<ProductViewModel>>(query);

            var paginationSet = new PaginationSet<ProductViewModel>()
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
        public IHttpActionResult Create(ProductViewModel productCategoryVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var newProduct = new Product();
                newProduct.UpdateProduct(productCategoryVm);
                newProduct.CreatedDate = DateTime.Now;
                newProduct.CreatedBy = User.Identity.Name;
                _productService.Add(newProduct);
                _productService.Save();

                var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);
                return Ok(responseData);
            }

        }

        [Route("update")]
        [HttpPut]
        public IHttpActionResult Update(ProductViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var dbProduct = _productService.GetById(productVm.ID);

                dbProduct.UpdateProduct(productVm);
                dbProduct.UpdatedDate = DateTime.Now;
                dbProduct.UpdatedBy = User.Identity.Name;
                _productService.Update(dbProduct);
                _productService.Save();

                var responseData = Mapper.Map<Product, ProductViewModel>(dbProduct);
                return Ok(responseData);
            }

        }

        [Route("delete")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var oldProductCategory = _productService.Delete(id);
                _productService.Save();

                var responseData = Mapper.Map<Product, ProductViewModel>(oldProductCategory);
                return Ok(responseData);
            }

        }

        [Route("deletemulti")]
        [HttpDelete]
        public IHttpActionResult DeleteMulti(string checkedProducts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedProducts);
                foreach (var item in listProductCategory)
                {
                    _productService.Delete(item);
                }

                _productService.Save();

                return Ok(listProductCategory.Count);
            }
        }

    }
}