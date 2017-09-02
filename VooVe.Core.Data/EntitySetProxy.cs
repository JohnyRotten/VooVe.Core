using System;
using System.Collections.Generic;
using System.Linq;

namespace VooVe.Core.Data
{
    internal class EntitySetProxy<TEntity> : IEntitySet<TEntity>
    {
        private readonly IEntitySet<TEntity> _entitySet;
        private IList<TEntity> _items = new List<TEntity>();
        private const int DefaultRefreshableTime = 10;
        private DateTime UpdatedTime { get; set; } = DateTime.Now;
        public TimeSpan RefreshableTime { get; set; } = TimeSpan.FromMinutes(DefaultRefreshableTime);

        protected IList<TEntity> Items
        {
            get
            {
                if ((DateTime.Now - UpdatedTime) > RefreshableTime || _items == null)
                {
                    Refresh();
                }
                return _items;
            }
        }

        public EntitySetProxy(IEntitySet<TEntity> entitySet)
        {
            _entitySet = entitySet;
        }

        public IEnumerable<TEntity> Get(Predicate<TEntity> condition)
        {
            return Items.Where(e => condition(e));
        }

        public void Add(TEntity entity)
        {
            _entitySet.Add(entity);
            Refresh();
        }

        public void Update(TEntity entity)
        {
            _entitySet.Update(entity);
            Refresh();
        }

        public void Remove(TEntity entity)
        {
            _entitySet.Remove(entity);
            Refresh();
        }

        public void Refresh()
        {
            _items = _entitySet.Get(_ => true).ToList();
            UpdatedTime = DateTime.Now;
        }

    }
}