using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductBrands.Any()) //Any=> One Element At Least Inside Collection
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                        await dbContext.ProductBrands.AddAsync(brand);

                    await dbContext.SaveChangesAsync();
                }
            }


            if (!dbContext.ProductTypes.Any()) //Any=> One Element At Least Inside Collection
            {
                var typesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                if (types?.Count > 0)
                {
                    foreach (var type in types)
                        await dbContext.ProductTypes.AddAsync(type);

                    await dbContext.SaveChangesAsync();
                }
            }


            if (!dbContext.Products.Any()) //Any=> One Element At Least Inside Collection
            {
                var productsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count > 0)
                {
                    foreach (var product in products)
                        await dbContext.Products.AddAsync(product);

                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.DeliveryMethods.Any()) //Any=> One Element At Least Inside Collection
            {
                var MethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(MethodsData);

                if (DeliveryMethods?.Count > 0)
                {
                    foreach (var Methods in DeliveryMethods)
                        await dbContext.DeliveryMethods.AddAsync(Methods);

                    await dbContext.SaveChangesAsync();
                }
            }

        }

    }
}
