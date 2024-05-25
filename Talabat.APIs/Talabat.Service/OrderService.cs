using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Spec;
using Talabat.Repository.Repositories;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentService paymentService;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShipToAddress)
        {
            //1.Get Basket From BasketRepo
            var basket=await basketRepository.GetBasketAsync(BasketId);

            //2.Get Selected Items At Basket From ProductRepo
            var OrderItems=new List<OrderItem>();
            
            if(basket?.Items?.Count>0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productOrderdItem = new ProductOrderItem(product.Id,product.Name,product.PictureUrl);
                    var orderItem = new OrderItem(productOrderdItem, product.Price, item.Quantity);
                    
                    OrderItems.Add(orderItem);
                }
            }

            //3.Calculate Subtotal
            var subTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            //4.Get DeliveryMethod From DeliveryMethodRepo
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            //5.Create Order
            var spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existOrder = await unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec); // بشوف انا عندي باسكت بنفس ال Payment ولا لا 

            if(existOrder is not null)
            {
                 unitOfWork.Repository<Order>().Delete(existOrder);  // لو موجود اخلق كونتكست يمسحه 
                await paymentService.CreateOrUpdatePaymentIntent(basket.Id); //هيعمل باسكت جديد وpayment جديدة
            }

            var Order=new Order(BuyerEmail, ShipToAddress,basket.PaymentIntentId, deliveryMethod, OrderItems,subTotal);

            //6.Add Order Locally 
            await unitOfWork.Repository<Order>().Add(Order);

            //7.Save Order To DataBase

            var Result = await unitOfWork.Complete();
            if (Result <= 0) return null;

            return Order;
            
        }

     

        public async Task<Order> GetOrderForUserByIdAsync(int OrderId, string BuyerEmail)
        {
            var spec = new OrderSpecification(OrderId, BuyerEmail);
            var order = await unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpecification(BuyerEmail);
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods;
        }
    }
}
