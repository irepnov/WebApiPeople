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
    public class PeopleRepository : IDBRepository<People, People>
    {
        private readonly TempRepnovIB_2Context _context;

        public PeopleRepository(TempRepnovIB_2Context context) => _context = context;

        public async Task<People> AddAsync(People entity)
        {
            _context.People.Add(entity);
            if (await _context.SaveChangesAsync() > 0)
                return entity;
            else
                return null;
        }

        public async Task<bool> DeleteAsync(People entity)
        {
            foreach (var del in entity.PeopleAdreses)
            {
                _context.Entry(del).State = EntityState.Deleted;
            }
            foreach (var del in entity.PeopleDocuments)
            {
                _context.Entry(del).State = EntityState.Deleted;
            }
            _context.People.Remove(entity);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<People> GetAsync(long id)
        {
            if (id == 0) throw new NullReferenceException("id empty");
           // _context.ChangeTracker.LazyLoadingEnabled = false;
            var people = await _context.People.
                Include(people => people.PeopleAdreses)
                    .ThenInclude(adres => adres.AdresType)
                .Include(peop => peop.PeopleDocuments)
                    .ThenInclude(doc => doc.DocumentType)
                .FirstOrDefaultAsync(item => item.Id == id);
            if (people == null)
                return null;

           /* _context.Entry(people)
                .Collection(b => b.PeopleAdreses)
                .Load();

            _context.Entry(people)
                // .Reference(b => b.PeopleDocuments)
                .Collection(b => b.PeopleDocuments)
                .Load();*/

            return people;
        }

        public IQueryable<People> GetAsQueryable(List<Expression<Func<People, bool>>> predicates = null, params Expression<Func<People, object>>[] includes)
        {
            IQueryable<People> query = _context.Set<People>().AsQueryable<People>();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, item) => EvaluateInclude(current, item));
            }

            Expression<Func<People, bool>> expresCombined = null;
            if (predicates?.Count > 0)
            {
                expresCombined = PredicateBuilder.True<People>(); //True для And / False для Or
                foreach (Expression<Func<People, bool>> item in predicates)
                {
                    expresCombined = expresCombined.And(item);
                }
            }

            return expresCombined != null ? query.Where(expresCombined).AsNoTracking() : query.AsNoTracking();
        }

        public async Task<IEnumerable<People>> GetAllAsync(List<Expression<Func<People, bool>>> predicates = null, params Expression<Func<People, object>>[] includes)
        {
            return await GetAsQueryable(predicates, includes).ToListAsync();
        }

        private IQueryable<People> EvaluateInclude(IQueryable<People> current, Expression<Func<People, object>> item)
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

        public async Task<People> GetDtoAsync(long id)
        {
            return await this.GetAsync(id);
        }

        public async Task<bool> UpdateAsync(People entity)
        {
            var entityToUpdate = await this.GetAsync(entity.Id);

            entityToUpdate.FirstName = entity.FirstName;
            entityToUpdate.MiddleName = entity.MiddleName;
            entityToUpdate.LastName = entity.LastName;
            entityToUpdate.BirthDate = entity.BirthDate;

            entityToUpdate.PeopleAdreses.ToList().ForEach(item => _context.PeopleAdreses.Remove(item));
            entity.PeopleAdreses.ToList().ForEach(item => _context.Entry(item).State = EntityState.Added);

            entityToUpdate.PeopleDocuments.ToList().ForEach(item => _context.PeopleDocuments.Remove(item));
            entity.PeopleDocuments.ToList().ForEach(item => _context.Entry(item).State = EntityState.Added);

            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
