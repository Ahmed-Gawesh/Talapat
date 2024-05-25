using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregation
{
    public class Order:BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress,string IntentId, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shippingAddress;
            PaymentIntentId = IntentId;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }= DateTimeOffset.Now; // الوقت اللي اتمل فيه ال order
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Default هو الانتظار

        public Address ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } //navegational prop (one) 

        public ICollection<OrderItem> Items { get; set; }= new HashSet<OrderItem>();

        public decimal SubTotal { get; set; }

        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; }
    }
}
