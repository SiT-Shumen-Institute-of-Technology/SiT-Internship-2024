using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SACS.Data.Common.Repositories;

namespace SACS.Data.Repositories;

public class EfRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    public EfRepository(ApplicationDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = Context.Set<TEntity>();
    }

    protected DbSet<TEntity> DbSet { get; set; }

    protected ApplicationDbContext Context { get; set; }

    public virtual IQueryable<TEntity> All()
    {
        return DbSet;
    }

    public virtual IQueryable<TEntity> AllAsNoTracking()
    {
        return DbSet.AsNoTracking();
    }

    public virtual Task AddAsync(TEntity entity)
    {
        return DbSet.AddAsync(entity).AsTask();
    }

    public virtual void Update(TEntity entity)
    {
        var entry = Context.Entry(entity);
        if (entry.State == EntityState.Detached) DbSet.Attach(entity);

        entry.State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public Task<int> SaveChangesAsync()
    {
        return Context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) Context?.Dispose();
    }
}