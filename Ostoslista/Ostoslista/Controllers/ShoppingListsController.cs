using Microsoft.AspNet.Identity;
using Ostoslista.Models;
using Ostoslista.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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

            // Omat listat
            var lists = _context.ShoppingLists
                            .Where(l => l.OwnerId == userId)
                            .Include(l => l.Items)
                            .Include(l => l.Owner)
                            .ToList();

            var ownListIds = lists.Select(l => l.Id);

            // Jaot kirjautuneelle käyttäjälle, lukuunottamatta omia listoja jotka mahdollisesti jaoissa
            var sharedLists = _context.ShoppingListShares
                                .Where(s => s.ReceiverUserId == userId && !ownListIds.Contains(s.ShoppingListId))
                                .Select(s => s.ShoppingList)
                                .Include(l => l.Items)
                                .Include(l => l.Owner)
                                .ToList();

            var vm = new ShoppingListIndexViewModel();

            vm.OwnShoppingLists = new List<ShoppingListViewModel>();

            foreach (var list in lists)
            {
                if (list.Name.Length > 30)
                {
                    list.Name = list.Name.Substring(0, 27) + "...";
                }

                vm.OwnShoppingLists.Add(new ShoppingListViewModel
                {
                    Id = list.Id,
                    Name = list.Name,
                    Items = list.Items,
                    OwnerName = list.Owner.UserName,
                    AddedDate = Utils.TimeConverter.ConvertToEetTimeString(list.Added)
                });
            }

            foreach (var list in sharedLists)
            {
                if (list.Name.Length > 30)
                {
                    list.Name = list.Name.Substring(0, 27) + "...";
                }

                vm.SharedShoppingLists.Add(new ShoppingListViewModel
                {
                    Id = list.Id,
                    Name = list.Name,
                    Items = list.Items,
                    OwnerName = list.Owner.UserName,
                    AddedDate = Utils.TimeConverter.ConvertToEetTimeString(list.Added),
                    EditAllowed = _context.ShoppingListShares.Any(s => s.ReceiverUserId == userId && s.ShoppingListId == list.Id
                                    && s.EditAllowed)
                });
            }

            ViewBag.Message = TempData["message"];

            return View(vm);
        }

        public ActionResult ViewList(int id)
        {
            return RedirectToAction("Index");
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
                    Added = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                };

                _context.ShoppingLists.Add(shoppingList);
                _context.SaveChanges();

                return RedirectToAction("Edit", new { id = shoppingList.Id });
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {
            var userId = User.Identity.GetUserId();
            var shoppingList = _context.ShoppingLists.Include(sl => sl.Items).SingleOrDefault(sl => sl.Id == id);

            if (shoppingList == null)
            {
                Response.StatusCode = 404;
                ViewBag.Message = "Ostoslistaa ei löytynyt";
                return View("Error");
            }

            var editAllowedForCurrentUser = _context.ShoppingListShares
                                                .Any(s => s.ShoppingListId == shoppingList.Id && s.ReceiverUserId == userId && s.EditAllowed);

            if (shoppingList.OwnerId != userId && !editAllowedForCurrentUser)
            {
                Response.StatusCode = 403;
                ViewBag.Message = "Ei oikeutta käsitellä ostoslistaa";
                return View("Error");
            }

            var vm = ShoppingListViewModel.Create(shoppingList);

            var shares = _context.ShoppingListShares
                            .Where(s => s.ShoppingListId == shoppingList.Id)
                            .Include(s => s.Receiver);

            List<ShowShareViewModel> shareVms = new List<ShowShareViewModel>();
            foreach (var share in shares)
            {
                shareVms.Add(new ShowShareViewModel
                {
                    UserId = share.ReceiverUserId,
                    UserName = share.Receiver.UserName
                });
            }

            vm.Shares = shareVms;
            vm.IsOwner = shoppingList.OwnerId == userId;

            return View(new ComboViewModel { ShoppingListViewModel = vm });
        }

        [HttpPost]
        public ActionResult Edit(ComboViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var list = _context.ShoppingLists.FirstOrDefault(s => s.Id == vm.ShoppingListViewModel.Id);

                if (list == null)
                {
                    Response.StatusCode = 404;
                    ViewBag.Message = "Ostoslistaa ei löytynyt";
                    return View("Error");
                }

                if (list.OwnerId != userId && !EditAllowedForCurrentUser(list))
                {
                    Response.StatusCode = 403;
                    ViewBag.Message = "Ei oikeutta käsitellä ostoslistaa";
                    return View("Error");
                }

                var item = new ShoppingListItem
                {
                    Name = vm.ShoppingListItemViewModel.Name,
                    Quantity = vm.ShoppingListItemViewModel.Quantity,
                    Added = DateTime.UtcNow,
                    ShoppingListId = vm.ShoppingListViewModel.Id
                };

                _context.ShoppingListItems.Add(item);
                _context.SaveChanges();

                return RedirectToAction("Edit", "ShoppingLists", new { id = vm.ShoppingListViewModel.Id });
            }

            var shoppingList = _context.ShoppingLists.Include(sl => sl.Items).SingleOrDefault(sl => sl.Id == vm.ShoppingListViewModel.Id);

            var comboVm = new ComboViewModel
            {
                ShoppingListViewModel = ShoppingListViewModel.Create(shoppingList)
            };

            return View(comboVm);
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

        [HttpPost]
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

            if (list.OwnerId != userId && !EditAllowedForCurrentUser(list))
            {
                Response.StatusCode = 403;
                ViewBag.Message = "Ei oikeutta poistaa ostosta";
                return View("Error");
            }

            _context.ShoppingListItems.Remove(item);
            _context.SaveChanges();

            return RedirectToAction("Edit", "ShoppingLists", new { id = list.Id });
        }

        public ActionResult View(int? id)
        {
            var userId = User.Identity.GetUserId();

            bool isOwnList = _context.ShoppingLists.Any(s => s.Id == id && s.OwnerId == userId);
            bool isAllowed = _context.ShoppingListShares.Any(s => s.ShoppingListId == id && s.ReceiverUserId == userId);

            // Onko oikeus tarkastella listaa
            if (isAllowed || isOwnList)
            {
                var list = _context.ShoppingLists.Include(s => s.Items).SingleOrDefault(s => s.Id == id);

                bool hasEditRight = _context.ShoppingListShares.Any(s => s.ShoppingListId == id && s.ReceiverUserId == userId && s.EditAllowed);

                // Onko oikeus muokata listaa
                if (isOwnList || hasEditRight)
                {
                    ViewBag.ShowEditLink = true;
                }
                else
                {
                    ViewBag.ShowEditLink = false;
                }

                return View(list);
            }

            Response.StatusCode = 403;
            ViewBag.Message = "Ei oikeutta katsella ostoslistaa";
            return View("Error");
        }

        [HttpPost]
        public ActionResult View(int itemId, int listId, bool bought)
        {
            var item = _context.ShoppingListItems.FirstOrDefault(i => i.Id == itemId);

            if (item.Bought && !bought)
            {
                item.Bought = false;
                item.TimeBought = null;
                _context.SaveChanges();

                return RedirectToAction("View", new { id = listId });
            }
            else if (!item.Bought && bought)
            {
                item.Bought = true;
                item.TimeBought = DateTime.UtcNow;
                _context.SaveChanges();

                return RedirectToAction("View", new { id = listId });
            }

            return View(listId);
        }

        private bool EditAllowedForCurrentUser(ShoppingList list)
        {
            var userId = User.Identity.GetUserId();
            return _context.ShoppingListShares.Any(s => s.ShoppingListId == list.Id && s.ReceiverUserId == userId && s.EditAllowed);
        }
    }
}