using System;
using System.Collections.Generic;

namespace VooVe.Core.Data
{
    public interface IEntitySet<TEntity>
    {
        IEnumerable<TEntity> Get(Predicate<TEntity> condition);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}