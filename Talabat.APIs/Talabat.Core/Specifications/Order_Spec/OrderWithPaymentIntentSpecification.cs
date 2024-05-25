﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Specifications;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderWithPaymentIntentSpecification : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpecification(string IntentId)
            : base(O => O.PaymentIntentId == IntentId)
        {

        }
    }
}
