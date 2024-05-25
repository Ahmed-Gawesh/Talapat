using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity // بعمل ميثود لاي حاجة بتورث منو
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T> GetByIdWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountWithSpec(ISpecification<T> spec);

        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
