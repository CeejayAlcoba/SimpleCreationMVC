using Microsoft.EntityFrameworkCore;
using ApplicationContexts;
using System.ComponentModel.DataAnnotations;
using EFCore.BulkExtensions;
using System.Reflection;
using System.Linq.Expressions;
using Utilities.Classes;
using Repositories.Interfaces;
using Models.Pagination;

namespace Repositories.Classes
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public virtual async Task<T?> InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<PagedResult<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10, T? filter = null)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();
        
            if (filter != null)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression? combined = null;
        
                foreach (var property in typeof(T).GetProperties())
                {
                    // Skip navigation/class properties to avoid runtime evaluation errors
                    if (property.PropertyType.IsClass && property.PropertyType != typeof(string)) continue;

                    var value = property.GetValue(filter);
                    if (value == null) continue; // Skip build checks for properties left null in the filter

                    var member = Expression.Property(parameter, property);
                    var constant = Expression.Constant(value, property.PropertyType);
        
                    var equalsCheck = Expression.Equal(member, constant);
        
                    combined = combined == null ? equalsCheck : Expression.AndAlso(combined, equalsCheck);
                }
        
                if (combined != null)
                {
                    var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
                    query = query.Where(lambda);
                }
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        
            return new PagedResult<T>
            {
                Items = items,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T?> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T?> DeleteByIdAsync(int id)
        {
            T? deletedData = await GetByIdAsync(id);
            if (deletedData != null)
            {
                _context.Set<T>().Remove(deletedData);
                await _context.SaveChangesAsync();
            }
            return deletedData;
        }

        public virtual async Task<IEnumerable<T>> BulkUpdateAsync(List<T> list)
        {
            if (list == null || !list.Any()) return list;

            await _context.BulkUpdateAsync(list);
            return list;
        }

        public virtual async Task<IEnumerable<T>> BulkInsertAsync(List<T> list)
        {
            if (list == null || !list.Any()) return list;

            await _context.BulkInsertAsync(list);
            return list;
        }

        public virtual async Task<IEnumerable<T>> BulkUpsertAsync(List<T> entities)
        {
            if (entities == null || !entities.Any()) return entities;

            await _context.BulkInsertOrUpdateAsync(entities);
            return entities;
        }

        public virtual async Task<IEnumerable<T>> BulkMergeAsync(List<T> entities, Expression<Func<T, bool>>? deleteFilter = null)
        {
            string keyName = GetKeyPropertyName();

            var keepIds = entities
                .Select(x => x.GetType().GetProperty(keyName)?.GetValue(x))
                .Where(id => id != null && Convert.ToInt32(id) > 0)
                .Cast<int>()
                .ToList();

            IQueryable<T> query = _context.Set<T>();

            if (deleteFilter != null)
            {
                query = query.Where(deleteFilter);
            }

            var entitiesToDelete = await query
                .Where(x => !keepIds.Contains(EF.Property<int>(x, keyName)))
                .ToListAsync();

            if (entitiesToDelete.Any())
            {
                await _context.BulkDeleteAsync(entitiesToDelete);
            }

            await BulkUpsertAsync(entities);

            return entities;
        }

        private string GetKeyPropertyName()
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            var primaryKey = entityType?.FindPrimaryKey();
            var keyProperty = primaryKey?.Properties.FirstOrDefault();

            if (keyProperty == null)
                throw new InvalidOperationException($"No Primary Key metadata mapped for entity type {typeof(T).Name}");

            return keyProperty.Name;
        }
    }
}