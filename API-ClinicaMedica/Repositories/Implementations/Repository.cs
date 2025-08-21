using API_ClinicaMedica.Data;
using Microsoft.EntityFrameworkCore;

namespace API_ClinicaMedica.Repositories.Implementations;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    public Repository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<T?>> GetAllAsync()
    {
        var all = await _context.Set<T>().ToListAsync();
        return all;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null)
        {
            throw new KeyNotFoundException("Entity not found.");
        }

        return entity;
    }

    public async Task AddAsync(T entity)
    {
       await _context.Set<T>().AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    { 
        _context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
         await _context.SaveChangesAsync();
    }
}