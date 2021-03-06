﻿using Ostoslista.Models;
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
        [Required(ErrorMessage = "Anna nimi ostoslistalle")]
        [StringLength(256, ErrorMessage = "Nimen pituus voi olla enintään {1} merkkiä")]
        public string Name { get; set; }

        public string OwnerName { get; set; }
        public bool IsOwner { get; set; }

        public string AddedDate { get; set; }
        public bool EditAllowed { get; set; }

        public ICollection<ShoppingListItem> Items { get; set; }

        public ICollection<ShowShareViewModel> Shares { get; set; }

        public static ShoppingListViewModel Create(ShoppingList shoppingList)
        {
            var vm = new ShoppingListViewModel
            {
                Id = shoppingList.Id,
                Name = shoppingList.Name,
                Items = shoppingList.Items
            };

            return vm;
        }
    }
}