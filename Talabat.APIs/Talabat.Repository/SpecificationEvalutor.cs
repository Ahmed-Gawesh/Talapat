using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvalutor<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;// context.products

            if (spec.Criteria is not null)//P=>P.Id==id
                query = query.Where(spec.Criteria);//context.products.Where(P=>P.Id==id)




            #region For Order(By / Descending) الترتيب
            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy); //context.products.OrderBy(P=>P.Name)

            if (spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);
            #endregion


            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);



            query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));
            //context.products.Where(P=>P.Id==id).Include(P=>P.ProductBrand).Include(P=>P.ProductType) بجيبها عن طريق ال id

            //context.products.OrderBy(P=>P.Name).Include(P=>P.ProductBrand).Include(P=>P.ProductType) الترتيب


            return query;
        }
    }
}
