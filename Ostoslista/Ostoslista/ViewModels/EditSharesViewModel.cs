using Ostoslista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ostoslista.ViewModels
{
    public class EditSharesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool EditAllowed { get; set; }
    }
}