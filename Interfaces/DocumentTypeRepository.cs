using Microsoft.EntityFrameworkCore;
using PeopleWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PeopleWebApi.Interfaces
{
    public class DocumentTypeRepository : IDBRepository<DocumentType, DocumentType>
    {
        private readonly TempRepnovIB_2Context _context;

        public DocumentTypeRepository(TempRepnovIB_2Context context) => _context = context;

        public async Task<DocumentType> AddAsync(DocumentType entity)
        {
            _context.DocumentType.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity;
            else
                return null;
        }

        public async Task<bool> DeleteAsync(DocumentType entity)
        {
            _context.DocumentType.Remove(entity);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<DocumentType> GetAsync(long id)
        {
            if (id == 0) throw new NullReferenceException("id empty");
            return await _context.DocumentType.FirstOrDefaultAsync(item => item.Id == id);
        }

        //public IQueryable<DocumentType> GetAsQueryable(List<Expression<Func<DocumentType, bool>>> predicates = null, , Func<IQueryable<DocumentType>, IIncludableQueryable<DocumentType, object>> include = null)
        public IQueryable<DocumentType> GetAsQueryable(List<Expression<Func<DocumentType, bool>>> predicates = null, params Expression<Func<DocumentType, object>>[] includes)
        {
            IQueryable<DocumentType> query = _context.Set<DocumentType>().AsQueryable<DocumentType>();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, item) => EvaluateInclude(current, item));
            }

            Expression<Func<DocumentType, bool>> expresCombined = null;
            if (predicates?.Count > 0)
            {
                expresCombined = PredicateBuilder.True<DocumentType>(); //True для And / False для Or
                foreach (Expression<Func<DocumentType, bool>> item in predicates)
                {
                    expresCombined = expresCombined.And(item);
                }
            }

            return expresCombined != null ? query.Where(expresCombined).AsNoTracking() : query.AsNoTracking();
        }

        private IQueryable<DocumentType> EvaluateInclude(IQueryable<DocumentType> current, Expression<Func<DocumentType, object>> item)
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

        public async Task<IEnumerable<DocumentType>> GetAllAsync(List<Expression<Func<DocumentType, bool>>> predicates = null, params Expression<Func<DocumentType, object>>[] includes)
        {
            return await GetAsQueryable(predicates, includes).ToListAsync();
        }

        public async Task<DocumentType> GetDtoAsync(long id)
        {
            return await this.GetAsync(id);
        }

        public async Task<bool> UpdateAsync(DocumentType entity)
        {
            _context.Entry(await this.GetAsync(entity.Id)).CurrentValues.SetValues(entity);
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
