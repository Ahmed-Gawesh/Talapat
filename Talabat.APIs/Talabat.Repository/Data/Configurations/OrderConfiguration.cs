using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShipToAddress, SA => SA.WithOwner()); //1 to 1 total

           
            builder.Property(O => O.Status)
                     .HasConversion(
                        OS => OS.ToString(), // بحولها ل string علشان الuser يفهمها 

                    OS => (OrderStatus)Enum.Parse(typeof(OrderStatus), OS) // بحولها ل enum علشان اسجلها في ال database 
                                );
           
            builder.Property(O => O.SubTotal)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(O => O.DeliveryMethod)
                  .WithMany()
                  .OnDelete(DeleteBehavior.SetNull);
           
            
            builder.HasMany(O => O.Items)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);  // علشان لما اجي امسح order يتمسح 
                                                       // ويمسح معاها ال FK اللي موجود في OrderItems
        }
    }
}
