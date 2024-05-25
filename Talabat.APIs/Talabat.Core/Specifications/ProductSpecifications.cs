using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductSpecifications:BaseSpecifications<Product>
    {
        public ProductSpecifications(ProductSpecParams productSpec)
            :base(P=>
                 (string.IsNullOrEmpty(productSpec.Search) ||P.Name.ToLower().Contains(productSpec.Search)) &&
                 (!productSpec.brandid.HasValue || P.ProductBrandId==productSpec.brandid) &&
                 (!productSpec.typeid.HasValue || P.ProductTypeId == productSpec.typeid)
               )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            if(!string.IsNullOrEmpty(productSpec.sort))
            {
                switch (productSpec.sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }


            ApplyPagination(productSpec.PageSize * (productSpec.PageIndex - 1), productSpec.PageSize);
        }
        public ProductSpecifications(int id):base(P=>P.Id==id) // 
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
