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

        [EmailAddress]
        [Required]
        [Display(Name = "Vastaanottajan sähköpostiosoite")]
        public string ReceiverEmail { get; set; }
    }
}