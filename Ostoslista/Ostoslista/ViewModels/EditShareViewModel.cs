using Ostoslista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ostoslista.ViewModels
{
    public class EditShareViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool EditAllowed { get; set; }
        public int ShareId { get; set; }
        public int ListId { get; set; }
        public bool IsOwner { get; set; }
    }
}