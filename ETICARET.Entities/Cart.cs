using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ETICARET.Entities
{
    public class Cart:BaseEntity
    {
        [MaxLength(450)]
        public required string UserId { get; set; }

        public List<CartItem> Items { get; set; }

        public decimal TotalPrice => Items.Sum(i => (i.Product?.Price ?? 0) * i.Quantity);

        public int TotalQuantity => Items.Sum(i => i.Quantity);
    }
}
