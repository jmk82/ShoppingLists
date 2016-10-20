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
        [Required(ErrorMessage = "Anna nimi ostokselle")]
        [StringLength(256, ErrorMessage = "Nimen pituus voi olla enintään {1} merkkiä")]
        public string Name { get; set; }

        [Display(Name = "Määrä")]
        [Range(1,int.MaxValue, ErrorMessage = "Ostoksen lukumäärä tulee olla välillä {2} - {1}")]
        public int Quantity { get; set; }

        public int ShoppingListId { get; set; }
    }
}