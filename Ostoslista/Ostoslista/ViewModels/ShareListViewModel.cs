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

        [EmailAddress(ErrorMessage = "Sähköpostiosoite ei ole kelvollinen")]
        [Required(ErrorMessage = "Anna vastaanottajan sähköpostiosoite")]
        [Display(Name = "Vastaanottajan sähköpostiosoite")]
        public string ReceiverEmail { get; set; }

        [Display(Name = "Salli muokkaus")]
        public bool EditAllowed { get; set; }

        public List<ShoppingListShare> Shares { get; set; }
    }
}