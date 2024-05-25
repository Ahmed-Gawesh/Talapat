using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> :ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get ; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set ; }
        public int Skip { get ; set; }
        public int Take { get ; set; }
        public bool IsPaginationEnabled { get; set ; }

        public BaseSpecifications()
        {
            
        }
        public BaseSpecifications(Expression<Func<T, bool>> criteria)
        {
            Criteria= criteria;
        }

        public void AddOrderBy(Expression<Func<T, object>> OrderBy)
        {
           this.OrderBy= OrderBy;
        }
        public void AddOrderByDescending(Expression<Func<T, object>> OrderByDescending)
        {
            this.OrderByDescending = OrderByDescending;
        }

        public void ApplyPagination(int skip,int take)
        {
            IsPaginationEnabled= true;
            this.Skip= skip;
            this.Take= take;
        }
    }
}
