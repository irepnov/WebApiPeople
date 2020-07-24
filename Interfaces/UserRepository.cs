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
    public class UserRepository : IDBRepository<User, User>
    {
        private readonly TempRepnovIB_2Context _context;

        public UserRepository(TempRepnovIB_2Context context) => _context = context;

        public async Task<User> AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetAsQueryable(List<Expression<Func<User, bool>>> predicates = null, params Expression<Func<User, object>>[] includes)
        {
            IQueryable<User> query = _context.Set<User>().AsQueryable<User>();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, item) => EvaluateInclude(current, item));
            }

            Expression<Func<User, bool>> expresCombined = null;
            if (predicates?.Count > 0)
            {
                expresCombined = PredicateBuilder.True<User>(); //True для And / False для Or
                foreach (Expression<Func<User, bool>> item in predicates)
                {
                    expresCombined = expresCombined.And(item);
                }
            }

            return expresCombined != null ? query.Where(expresCombined).AsNoTracking() : query.AsNoTracking();
        }

        private IQueryable<User> EvaluateInclude(IQueryable<User> current, Expression<Func<User, object>> item)
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

        public async Task<IEnumerable<User>> GetAllAsync(List<Expression<Func<User, bool>>> predicates = null, params Expression<Func<User, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetDtoAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
