using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ETICARET.Entities
{
    public class Product:BaseEntity
    {
        [MaxLength(200)]
        public required string Name { get; set; }

        [MaxLength(4000)]
        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        //Anasayfada "Öne Çıkanlar" bölümünde gösterilecek mi?
        public bool IsHome { get; set; }

        //OneToMany İlişki
        public List<Image> Images { get; set; }


        //ManyToMany İlişki
        public List<Category> Categories { get; set; }

        //public List<Comment> Comments { get; set; }
    }
}
