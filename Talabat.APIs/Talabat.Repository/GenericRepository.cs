using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext dbContxt;

        public GenericRepository(StoreContext dbContxt)
        {
            this.dbContxt = dbContxt;
        }
        #region StaticQueris
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await dbContxt.Set<T>().ToListAsync();

        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContxt.Set<T>().FindAsync(id);
        } 
        #endregion

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

      

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(dbContxt.Set<T>(), spec);

        }


        //For Add ,Update , Delete => Locally 
        public async Task Add(T entity)
         => await dbContxt.Set<T>().AddAsync(entity);

        public void Update(T entity)
         => dbContxt.Set<T>().Update(entity);

        public void Delete(T entity)
         => dbContxt.Set<T>().Remove(entity); 
    }
}
