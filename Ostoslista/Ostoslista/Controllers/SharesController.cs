using Microsoft.AspNet.Identity;
using Ostoslista.Models;
using Ostoslista.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ostoslista.Controllers
{
    [Authorize]
    public class SharesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SharesController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult Create(int? id)
        {
            var userId = User.Identity.GetUserId();
            var list = _context.ShoppingLists.SingleOrDefault(s => s.Id == id);

            if (list == null)
            {
                Response.StatusCode = 404;
                ViewBag.Message = "Ostoslistaa ei löytynyt";
                return View("Error");
            }

            var editAllowed = _context.ShoppingListShares.Any(s => s.ShoppingListId == list.Id && s.ReceiverUserId == userId && s.EditAllowed);

            if (list.OwnerId != userId && !editAllowed)
            {
                Response.StatusCode = 403;
                ViewBag.Message = "Ei oikeutta tarkastella ostoslistaa";
                return View("Error");
            }

            var vm = new ShareListViewModel
            {
                ShoppingListId = list.Id,
                Name = list.Name
            };

            return View(vm);
        }

        [HttpPost]
        public ActionResult Create(ShareListViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var receiver = _context.Users.SingleOrDefault(u => u.UserName == vm.ReceiverUsername);

                if (receiver == null)
                {
                    ModelState.AddModelError("ReceiverUsername", "Käyttäjää ei löydy kyseisellä nimellä. Tarkista käyttäjänimi.");

                    return View(vm);
                }

                var shareExists = _context.ShoppingListShares
                                    .Any(s => s.ShoppingListId == vm.ShoppingListId && s.ReceiverUserId == receiver.Id);

                if (shareExists)
                {
                    ModelState.AddModelError("", "Lista on jo jaettu kyseiselle käyttäjälle");

                    return View(vm);
                }

                var share = new ShoppingListShare
                {
                    ShoppingListId = vm.ShoppingListId,
                    ReceiverUserId = receiver.Id,
                    EditAllowed = vm.EditAllowed
                };

                _context.ShoppingListShares.Add(share);
                _context.SaveChanges();

                TempData["message"] = "Lista '" + vm.Name + "' jaettu käyttäjälle " + vm.ReceiverUsername;

                return RedirectToAction("Index", "ShoppingLists");
            }

            return View(vm);
        }

        public ActionResult Edit(int? id)
        {
            var shares = _context.ShoppingListShares
                            .Where(s => s.ShoppingListId == id)
                            .Include(s => s.Receiver)
                            .Include(s => s.ShoppingList)
                            .ToList();

            List<EditShareViewModel> shareVms = new List<EditShareViewModel>();

            foreach (var share in shares)
            {
                var isOwner = share.ReceiverUserId == share.ShoppingList.OwnerId;

                if (!isOwner)
                {
                    shareVms.Add(new EditShareViewModel
                    {
                        UserId = share.ReceiverUserId,
                        UserName = share.Receiver.UserName,
                        EditAllowed = share.EditAllowed,
                        ShareId = share.Id,
                        ListId = share.ShoppingListId,
                    });
                }
            }

            var list = _context.ShoppingLists.FirstOrDefault(s => s.Id == id);
            ViewBag.ListName = list.Name;
            ViewBag.ListId = list.Id;
            ViewBag.Message = TempData["Message"];

            return View(shareVms);
        }

        [HttpPost]
        public ActionResult Edit(int listId, int shareId, bool allowEdit)
        {
            var userId = User.Identity.GetUserId();

            var share = _context.ShoppingListShares.Include(s => s.Receiver).FirstOrDefault(s => s.Id == shareId);
            share.EditAllowed = allowEdit;
            _context.SaveChanges();

            TempData["Message"] = string.Format("Käyttäjän '{0}' oikeuksia muutettu", share.Receiver.UserName);

            return RedirectToAction("Edit", new { id = listId });
        }

        [HttpPost]
        public ActionResult Delete(int listId, int shareId)
        {
            var userId = User.Identity.GetUserId();
            bool isOwnList = _context.ShoppingLists.Any(s => s.Id == listId && s.OwnerId == userId);

            if (isOwnList)
            {
                var share = _context.ShoppingListShares.FirstOrDefault(s => s.Id == shareId);
                _context.ShoppingListShares.Remove(share);
                _context.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = listId });
        }
    }
}