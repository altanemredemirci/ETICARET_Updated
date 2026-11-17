using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ETICARET.Entities
{
    public class CartItem:BaseEntity
    {

        //Mapping 1-N İlişkisi için CartId ve Cart propertylerini ekledik.
        public int CartId { get; set; }
        public Cart? Cart { get; set; }


        //1-N İlişkisi için ProductId ve Product propertylerini ekledik.
        public int ProductId { get; set; }
        public Product? Product { get; set; }


        public int Quantity { get; set; }
    }
}
