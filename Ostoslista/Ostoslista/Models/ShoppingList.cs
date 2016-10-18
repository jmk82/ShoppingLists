using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ostoslista.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public ApplicationUser Owner { get; set; }
        public string Name { get; set; }
        public DateTime Added { get; set; }
        public DateTime Updated { get; set; }
        public ICollection<ShoppingListItem> Items { get; set; }
    }
}