using System;
using System.ComponentModel.DataAnnotations;

namespace Ostoslista.Models
{
    public class ShoppingListItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int Quantity { get; set; }
        public DateTime Added { get; set; }
        public bool Bought { get; set; }
        public DateTime? TimeBought { get; set; }
        public int ShoppingListId { get; set; }
    }
}