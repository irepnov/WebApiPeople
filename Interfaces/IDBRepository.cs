using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PeopleWebApi.Interfaces
{
    public interface IDBRepository<TEntity, TDto>
    {
        IQueryable<TEntity> GetAsQueryable(List<Expression<Func<TEntity, bool>>> predicates = null, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAllAsync(List<Expression<Func<TEntity, bool>>> predicates = null, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetAsync(long id);
        Task<TEntity> GetDtoAsync(long id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
    }
}
