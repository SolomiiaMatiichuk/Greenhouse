using Microsoft.EntityFrameworkCore;
using PhotoZoneBooking.DAL.Entities;
using PhotoZoneBooking.DAL.Interfaces;

namespace PhotoZoneBooking.DAL.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly DataContext _context;
    private DbSet<T> _entities;
    
    public GenericRepository(DataContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }
    
    public void DetachLocal<T>(T t, int entryId) where T : BaseEntity
    {
        var local = _context.Set<T>()
                            .Local
                            .FirstOrDefault(entry => entry.Id.Equals(entryId));
        if (local is not null)
        {
            _context.Entry(local).State = EntityState.Detached;
        }
        _context.Entry(t).State = EntityState.Modified;
    }

    public virtual IQueryable<T> QueryWithNavigationFields()
    {
        var query = _entities.AsQueryable();
        var navigations = _context.Model.FindEntityType(typeof(T))?
                                                            .GetDerivedTypesInclusive()
                                                            .SelectMany(t => t.GetNavigations())
                                                            .Distinct();

        if (navigations != null)
        {
            query = navigations.Aggregate(query, (current, property) 
                => current.Include(property.Name));
        }

        return query;
    }

    public async Task<List<T>> GetAllAsync() => await QueryWithNavigationFields()
                                                      .AsNoTracking()
                                                      .ToListAsync();

    public async Task<T?> GetByIdAsync(int id)
    {
        var result = await QueryWithNavigationFields()
                           .AsNoTracking()
                           .SingleOrDefaultAsync(t => t.Id == id);
        return result;
    } 

    public async Task InsertAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException("Entity is null");
        }

        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException("Entity is null");
        }

        var oldEntity = await QueryWithNavigationFields().SingleOrDefaultAsync(t => t.Id == entity.Id);
        _context.Entry(oldEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException("Entity is null");
        }

        _entities.Remove(entity);
        await _context.SaveChangesAsync();
    }
}