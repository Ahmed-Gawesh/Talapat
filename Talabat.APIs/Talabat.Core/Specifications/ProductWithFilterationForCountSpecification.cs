using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFilterationForCountSpecification:BaseSpecifications<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams productSpec)
               : base(P =>
                    (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search)) &&
                    (!productSpec.brandid.HasValue || P.ProductBrandId == productSpec.brandid) &&
                    (!productSpec.typeid.HasValue || P.ProductTypeId == productSpec.typeid)
                  )
        {

        }
    }
}
