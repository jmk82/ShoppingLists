using Microsoft.AspNet.Identity;
using Ostoslista.Models;
using Ostoslista.ViewModels;
using System;
using System.Collections.Generic;
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

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var shares = _context.ShoppingListShares.Where(s => s.ReceiverUserId == userId).ToList();

            List<ShoppingList> lists = new List<ShoppingList>();

            foreach (var share in shares)
            {
                lists.Add(_context.ShoppingLists.SingleOrDefault(s => s.Id == share.ShoppingListId));
            }

            return View(lists);
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
            if (list.OwnerId != userId)
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
                var receiver = _context.Users.SingleOrDefault(u => u.Email == vm.ReceiverEmail);

                if (receiver == null)
                {
                    ModelState.AddModelError("ReceiverEmail", "Käyttäjää ei löydy kyseisellä sähköpostiosoitteella. Tarkista osoite.");

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

                TempData["message"] = "Lista '" + vm.Name + "' jaettu käyttäjälle " + vm.ReceiverEmail;

                return RedirectToAction("Index", "ShoppingLists");
            }

            return View(vm);
        }
    }
}