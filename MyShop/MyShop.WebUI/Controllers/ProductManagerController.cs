using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        InMemoryRepository<Product> context;

        InMemoryRepository<ProductCategory> productCategoryRepository;

        public ProductManagerController()
        {
            context = new InMemoryRepository<Product>();
            productCategoryRepository = new InMemoryRepository<ProductCategory>();
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel productManagerViewModel = new ProductManagerViewModel
            {
                Product = new Product(),
                ProductCategories = productCategoryRepository.Collection()
            };

            return View(productManagerViewModel);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                context.Insert(product);
                context.Commit();
                return RedirectToAction("Index");
            }

        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel productManagerViewModel = new ProductManagerViewModel();
                productManagerViewModel.Product = product;
                productManagerViewModel.ProductCategories = productCategoryRepository.Collection();

                return View(productManagerViewModel);
            }
        }
        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToUpdate = context.Find(Id);
            if(productToUpdate == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                productToUpdate.Category = product.Category;
                productToUpdate.Description = product.Description;
                productToUpdate.Image = product.Image;
                productToUpdate.Name = product.Name;
                productToUpdate.Price = product.Price;

                context.Commit();

                return RedirectToAction("Index");
            }
            
        }
        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if(productToDelete == null){
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}