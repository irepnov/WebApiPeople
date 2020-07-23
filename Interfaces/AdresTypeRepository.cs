using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PeopleWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PeopleWebApi.Interfaces
{
    public class AdresTypeRepository : IDBRepository<AdresType, AdresType>
    {
        private readonly TempRepnovIB_2Context _context;

        public AdresTypeRepository(TempRepnovIB_2Context context) => _context = context;

        public async Task<AdresType> AddAsync(AdresType entity)
        {
            _context.AdresType.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity;
            else
                return null;
        }

        public async Task<bool> DeleteAsync(AdresType entity)
        {
            _context.AdresType.Remove(entity);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<AdresType> GetAsync(long id)
        {
            if (id == 0) throw new NullReferenceException("id empty");
            return await _context.AdresType.FirstOrDefaultAsync(item => item.Id == id);
        }

        public IQueryable<AdresType> GetAsQueryable(List<Expression<Func<AdresType, bool>>> predicates = null, params Expression<Func<AdresType, object>>[] includes)
        {
            IQueryable<AdresType> query = _context.Set<AdresType>().AsQueryable<AdresType>();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, item) => EvaluateInclude(current, item));
            }

            Expression<Func<AdresType, bool>> expresCombined = null;
            if (predicates?.Count > 0)
            {
                expresCombined = PredicateBuilder.True<AdresType>(); //True для And / False для Or
                foreach (Expression<Func<AdresType, bool>> item in predicates)
                {
                    expresCombined = expresCombined.And(item);
                }
            }

            return expresCombined != null ? query.Where(expresCombined).AsNoTracking() : query.AsNoTracking();
        }

        private IQueryable<AdresType> EvaluateInclude(IQueryable<AdresType> current, Expression<Func<AdresType, object>> item)
        {
            if (item.Body is MethodCallExpression)
            {
                var arguments = ((MethodCallExpression)item.Body).Arguments;
                if (arguments.Count > 1)
                {
                    var navigationPath = string.Empty;
                    for (var i = 0; i < arguments.Count; i++)
                    {
                        var arg = arguments[i];
                        var path = arg.ToString().Substring(arg.ToString().IndexOf('.') + 1);

                        navigationPath += (i > 0 ? "." : string.Empty) + path;
                    }
                    return current.Include(navigationPath);
                }
            }

            return current.Include(item);
        }

        public async Task<IEnumerable<AdresType>> GetAllAsync(List<Expression<Func<AdresType, bool>>> predicates = null, params Expression<Func<AdresType, object>>[] includes)
        {
            return await GetAsQueryable(predicates, includes).ToListAsync();
        }

        public async Task<AdresType> GetDtoAsync(long id)
        {
            return await this.GetAsync(id);
        }

        public async Task<bool> UpdateAsync(AdresType entity)
        {
            _context.Entry(await this.GetAsync(entity.Id)).CurrentValues.SetValues(entity);
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
