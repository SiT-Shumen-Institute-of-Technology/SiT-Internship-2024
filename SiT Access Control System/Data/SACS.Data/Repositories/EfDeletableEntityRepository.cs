using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SACS.Data.Common.Models;
using SACS.Data.Common.Repositories;

namespace SACS.Data.Repositories;

public class EfDeletableEntityRepository<TEntity> : EfRepository<TEntity>, IDeletableEntityRepository<TEntity>
    where TEntity : class, IDeletableEntity
{
    public EfDeletableEntityRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public override IQueryable<TEntity> All()
    {
        return base.All().Where(x => !x.IsDeleted);
    }

    public override IQueryable<TEntity> AllAsNoTracking()
    {
        return base.AllAsNoTracking().Where(x => !x.IsDeleted);
    }

    public IQueryable<TEntity> AllWithDeleted()
    {
        return base.All().IgnoreQueryFilters();
    }

    public IQueryable<TEntity> AllAsNoTrackingWithDeleted()
    {
        return base.AllAsNoTracking().IgnoreQueryFilters();
    }

    public void HardDelete(TEntity entity)
    {
        base.Delete(entity);
    }

    public void Undelete(TEntity entity)
    {
        entity.IsDeleted = false;
        entity.DeletedOn = null;
        Update(entity);
    }

    public override void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedOn = DateTime.UtcNow;
        Update(entity);
    }
}