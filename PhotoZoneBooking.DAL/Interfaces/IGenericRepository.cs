using PhotoZoneBooking.DAL.Entities;

namespace PhotoZoneBooking.DAL.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    void DetachLocal<T>(T t, int entryId) where T : BaseEntity;
}