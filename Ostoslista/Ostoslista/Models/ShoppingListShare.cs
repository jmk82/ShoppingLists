using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ostoslista.Models
{
    public class ShoppingListShare
    {
        public int Id { get; set; }
        public int ShoppingListId { get; set; }
        public string ReceiverUserId { get; set; }
    }
}