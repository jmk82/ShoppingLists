using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ostoslista.ViewModels
{
    public class ShoppingListIndexViewModel
    {
        public ICollection<ShoppingListViewModel> OwnShoppingLists { get; set; }
        public ICollection<ShoppingListViewModel> SharedShoppingLists { get; set; }

        public ShoppingListIndexViewModel()
        {
            OwnShoppingLists = new List<ShoppingListViewModel>();
            SharedShoppingLists = new List<ShoppingListViewModel>();
        }
    }
}