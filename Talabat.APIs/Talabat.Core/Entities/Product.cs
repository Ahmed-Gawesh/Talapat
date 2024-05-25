using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal  Price { get; set; }


        public int ProductBrandId { get; set; } //ForigenKey
        public ProductBrand ProductBrand { get; set; } // Relation One 

        public int ProductTypeId { get; set; }//ForigenKey
        public ProductType ProductType { get; set; }  // Relation One
    }
}
