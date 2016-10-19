using Ostoslista.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ostoslista.ViewModels
{
    public class ShoppingListItemViewModel
    {
        public string Name { get; set; }

        [Display(Name = "Määrä")]
        public int Quantity { get; set; }

        public int ShoppingListId { get; set; }
    }
}