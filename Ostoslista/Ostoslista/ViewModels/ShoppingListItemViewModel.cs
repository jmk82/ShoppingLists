using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ostoslista.ViewModels
{
    public class ShoppingListItemViewModel
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int ShoppingListId { get; set; }
    }
}