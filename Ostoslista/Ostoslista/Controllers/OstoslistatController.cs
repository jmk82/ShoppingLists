﻿using Microsoft.AspNet.Identity;
using Ostoslista.Models;
using Ostoslista.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Ostoslista.Controllers
{
    public class OstoslistatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OstoslistatController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Ostoslistat
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var lists = _context.ShoppingLists.Where(l => l.OwnerId == userId).Include(sl => sl.Items).ToList();

            return View(lists);
        }

        [Authorize]
        public ActionResult Luo()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Luo(ShoppingListViewModel vm)
        {
            var shoppingList = new ShoppingList
            {
                OwnerId = User.Identity.GetUserId(),
                Name = vm.Name,
                Added = DateTime.Now,
                Updated = DateTime.Now
            };

            _context.ShoppingLists.Add(shoppingList);
            _context.SaveChanges();

            return RedirectToAction("Muokkaa", new { id = shoppingList.Id });
        }

        [Authorize]
        public ActionResult Muokkaa(int id)
        {
            var userId = User.Identity.GetUserId();
            var shoppingList = _context.ShoppingLists.Include(sl => sl.Items).SingleOrDefault(sl => sl.Id == id);

            if (shoppingList == null)
            {
                Response.StatusCode = 404;
                ViewBag.Message = "Ostoslistaa ei löytynyt";
                return View("Error"); ;
            }
            if (shoppingList.OwnerId != userId)
            {
                Response.StatusCode = 403;
                ViewBag.Message = "Ei oikeutta tarkastella ostoslistaa";
                return View("Error");
            }

            var vm = new ShoppingListViewModel
            {
                Id = shoppingList.Id,
                Name = shoppingList.Name,
                Items = shoppingList.Items
            };

            return View(vm);
        }
    }
}