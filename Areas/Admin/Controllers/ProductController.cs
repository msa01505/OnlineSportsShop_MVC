using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proj.Areas.Admin.Services;
using Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "WAdmin")]
    public class ProductController : Controller
    {

        private readonly CRUDRepo<Product> service;
        public ProductController(CRUDRepo<Product> service)
        {
            this.service = service;
        }

        // GET: Admin/Clothes
        public IActionResult Index()
        {
            return View(service.GetAll());
        }

        //// GET: Admin/Clothes/Details/5
        public IActionResult Details(int id)
        {
            return View(service.GetDetails(id));
        }


        // GET: Admin/Clothes/Create
        public IActionResult Create()
        {
            return View();
        }

        //// POST: Admin/Clothes/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if(product.Source == null)
            {
                product.Source = "main.jpg";
               
            }
            service.Insert(product);
            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/Clothes/Edit/5
        public IActionResult Edit(int id)
        {

            return View(service.GetDetails(id));
        }

        // POST: Admin/Clothes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {



            try
            {
                service.Update(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ///////////////////////////////
                return View("Error");
            }


        }

        // GET: Admin/Clothes/Delete/5
        public IActionResult Delete(int id)
        {
            service.Delete(id);
            return RedirectToAction(nameof(Index));

        }
    }
}
