﻿using Microsoft.Extensions.Configuration;
using Stripe;
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
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["Stripe:SecreteKey"];
            var basket = await basketRepository.GetBasketAsync(basketId);

            if (basket is null) return null;

            var ShippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            { 
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                ShippingPrice = deliveryMethod.Cost;
                basket.ShippingCost = ShippingPrice;
            }

            if(basket?.Items?.Count>0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(item.Price!=product.Price)
                        item.Price = product.Price;
                }
            }

            var service = new PaymentIntentService();

            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(basket.PaymentIntentId)) //Create PaymentIntent 
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)basket.ShippingCost * 100, //بضرب في 100 علشان بتعامل مع سينتات 
                    Currency = "usd",
                    PaymentMethodTypes=new List<string> { "card"}
                };
                paymentIntent = await service.CreateAsync(options);
                
                basket.ClientSecret = paymentIntent.ClientSecret;
                basket.PaymentIntentId = paymentIntent.Id;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)basket.ShippingCost * 100, //بضرب في 100 علشان بتعامل مع سينتات 
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,options);

            }

            await basketRepository.UpdateBasketAsync(basket);

            return basket;
        }
    }
}