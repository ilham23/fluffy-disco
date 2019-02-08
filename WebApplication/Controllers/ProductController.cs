using AutoMapper;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using PagedList;
using System.Net;
using System.Data.Entity.Infrastructure;
using Model.Models;
using WebApplication.Infrastructure.Extensions;

namespace WebApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        public ProductController(IProductService productService, IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
    }
        // GET: Product
        public ActionResult Index(string searchString)
        {
            ViewBag.CurrentFilter = searchString;
            ViewBag.ListCategory = _productCategoryService.GetAll().ToList();
            var products = Mapper.Map<ICollection<ProductViewModel>>(_productService.GetListProduct(searchString));

            return View(products);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var productEntity = _productService.GetById(id);
            var product = Mapper.Map<ProductViewModel>(productEntity);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCategory = _productCategoryService.GetAll().ToList();
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind]ProductViewModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var productEntity = Mapper.Map<Product>(product);
                    _productService.Add(productEntity);
                   
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(product);
        }


        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var product = Mapper.Map<ProductViewModel>(_productService.GetById(id));
            ViewBag.ListCategory = _productCategoryService.GetAll().ToList();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind]ProductViewModel product)
        {
                var productEntity = _productService.GetById(id);
                if (productEntity == null)
                {
                    return HttpNotFound();
                }
                EntityExtensions.UpdateProduct(ref productEntity, product);
                try
                {
                    _productService.Update(productEntity);

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
                // TODO: Add update logic here
                return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id, bool? saveChangesError = false)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            var product = Mapper.Map<ProductViewModel>(_productService.GetById(id));
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var product = _productService.GetById(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                _productService.Delete(id);
            }
            catch (RetryLimitExceededException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }
    }
}
