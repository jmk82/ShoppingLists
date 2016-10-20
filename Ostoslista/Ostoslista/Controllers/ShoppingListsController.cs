using Microsoft.AspNet.Identity;
using Ostoslista.Models;
using Ostoslista.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Ostoslista.Controllers
{
    [Authorize]
    public class ShoppingListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingListsController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var lists = _context.ShoppingLists.Where(l => l.OwnerId == userId).Include(sl => sl.Items).ToList();

            return View(lists);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ShoppingListViewModel vm)
        {
            if (ModelState.IsValid)
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

                return RedirectToAction("Edit", new { id = shoppingList.Id });
            }

            return View();
        }

        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var shoppingList = _context.ShoppingLists.Include(sl => sl.Items).SingleOrDefault(sl => sl.Id == id);

            if (shoppingList == null)
            {
                Response.StatusCode = 404;
                ViewBag.Message = "Ostoslistaa ei löytynyt";
                return View("Error");
            }
            if (shoppingList.OwnerId != userId)
            {
                Response.StatusCode = 403;
                ViewBag.Message = "Ei oikeutta tarkastella ostoslistaa";
                return View("Error");
            }

            var vm = CreateShoppingListViewModel(shoppingList);

            return View(new ComboViewModel { ShoppingListViewModel = vm });
        }

        [HttpPost]
        public ActionResult Edit(ComboViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var item = new ShoppingListItem
                {
                    Name = vm.ShoppingListItemViewModel.Name,
                    Quantity = vm.ShoppingListItemViewModel.Quantity,
                    Added = DateTime.Now,
                    ShoppingListId = vm.ShoppingListViewModel.Id
                };

                _context.ShoppingListItems.Add(item);
                _context.SaveChanges();

                return RedirectToAction("Edit", "ShoppingLists", new { id = vm.ShoppingListViewModel.Id });
            }

            var shoppingList = _context.ShoppingLists.Include(sl => sl.Items).SingleOrDefault(sl => sl.Id == vm.ShoppingListViewModel.Id);
            var comboVm = new ComboViewModel
            {
                ShoppingListViewModel = CreateShoppingListViewModel(shoppingList)
            };

            return View(comboVm);
        }

        private ShoppingListViewModel CreateShoppingListViewModel(ShoppingList shoppingList)
        {
            var vm = new ShoppingListViewModel
            {
                Id = shoppingList.Id,
                Name = shoppingList.Name,
                Items = shoppingList.Items
            };

            return vm;
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();
            var list = _context.ShoppingLists.SingleOrDefault(s => s.Id == id);

            if (list == null)
            {
                Response.StatusCode = 404;
                ViewBag.Message = "Ostoslistaa ei löytynyt";
                return View("Error");
            }
            if (list.OwnerId != userId)
            {
                Response.StatusCode = 403;
                ViewBag.Message = "Ei oikeutta poistaa ostoslistaa";
                return View("Error");
            }

            _context.ShoppingLists.Remove(list);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult DeleteItem(int id)
        {
            var userId = User.Identity.GetUserId();
            var item = _context.ShoppingListItems.SingleOrDefault(i => i.Id == id);

            if (item == null)
            {
                Response.StatusCode = 404;
                ViewBag.Message = "Ostosta ei löytynyt";
                return View("Error");
            }

            var list = _context.ShoppingLists.SingleOrDefault(s => s.Id == item.ShoppingListId);

            if (list.OwnerId != userId)
            {
                Response.StatusCode = 403;
                ViewBag.Message = "Ei oikeutta poistaa ostosta";
                return View("Error");
            }

            _context.ShoppingListItems.Remove(item);
            _context.SaveChanges();

            return RedirectToAction("Edit", "ShoppingLists", new { id = list.Id });
        }
    }
}