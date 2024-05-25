using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address shippingAddress);

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string BuyerEmail);

        Task<Order> GetOrderForUserByIdAsync(int OrderId, string BuyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethods();
    }
}
