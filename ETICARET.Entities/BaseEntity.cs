using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
