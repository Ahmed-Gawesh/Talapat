using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderSpecification:BaseSpecifications<Order>
    {
        public OrderSpecification(string email)
            :base(O=>O.BuyerEmail== email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            AddOrderByDescending(O => O.OrderDate);  // بيرتبهم باحدث وقت 

            
        }

        public OrderSpecification(int id,string email)
        : base(O => O.BuyerEmail == email && O.Id==id)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
