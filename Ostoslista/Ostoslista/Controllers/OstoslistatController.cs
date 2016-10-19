using Microsoft.AspNet.Identity;
using Ostoslista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var user = _context.Users.Single(u => u.Id == userId);
            var lists = _context.ShoppingLists.Where(l => l.Owner.Id == user.Id).ToList();

            return View(lists);
        }
    }
}