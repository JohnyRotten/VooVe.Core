using System;
using System.Collections.Generic;

namespace VooVe.Core.Data
{
    public interface IEntitySet<TEntity>
    {
        TEntity Get(Predicate<TEntity> condition);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        IEnumerable<TEntity> GetAll();
    }
}