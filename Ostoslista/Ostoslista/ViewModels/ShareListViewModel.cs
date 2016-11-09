using Ostoslista.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ostoslista.ViewModels
{
    public class ShareListViewModel
    {
        public int ShoppingListId { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "Anna vastaanottajan käyttäjänimi")]
        [Display(Name = "Vastaanottajan käyttäjänimi")]
        public string ReceiverUsername { get; set; }

        [Display(Name = "Salli muokkaus")]
        public bool EditAllowed { get; set; }

        public List<ShoppingListShare> Shares { get; set; }
    }
}