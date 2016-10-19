using Ostoslista.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ostoslista.ViewModels
{
    public class ShoppingListViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Listan nimi")]
        public string Name { get; set; }

        public ICollection<ShoppingListItem> Items { get; set; }

        public ShoppingListItemViewModel newItem { get; set; }
    }
}