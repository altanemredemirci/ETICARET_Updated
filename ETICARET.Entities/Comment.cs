using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ETICARET.Entities
{
    public class Comment:BaseEntity
    {
        [MaxLength(1000)]
        public required string Text { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public required string UserId { get; set; }

        [MaxLength(256)]
        public required string Username { get; set; }
    }
}
