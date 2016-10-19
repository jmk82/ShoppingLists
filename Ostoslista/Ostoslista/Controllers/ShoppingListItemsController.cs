using Ostoslista.Models;
using Ostoslista.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ostoslista.Controllers
{
    //[Authorize]
    public class ShoppingListItemsController : ApiController
    {
        private ApplicationDbContext _context;

        public ShoppingListItemsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Create(ShoppingListItemViewModel vm)
        {
            var item = new ShoppingListItem
            {
                Name = vm.Name,
                Quantity = vm.Quantity,
                Added = DateTime.Now,
                ShoppingListId = vm.ShoppingListId
            };

            _context.ShoppingListItems.Add(item);
            _context.SaveChanges();

            return Ok();
        }
    }
}
