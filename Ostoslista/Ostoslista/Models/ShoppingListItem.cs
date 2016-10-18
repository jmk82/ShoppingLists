using System;

namespace Ostoslista.Models
{
    public class ShoppingListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime Added { get; set; }
        public bool Striked { get; set; }
    }
}