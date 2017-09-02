using System;
using System.Collections.Generic;

namespace VooVe.Core.Data
{
    public class EntityProviderProxy : IEntityProvider
    {
        private readonly IEntityProvider _provider;

        private readonly IDictionary<Type, Func<object>> _proxyMap = new Dictionary<Type, Func<object>>();

        public EntityProviderProxy(IEntityProvider provider)
        {
            _provider = provider;
        }
        
        public IEntitySet<TEntity> Set<TEntity>()
        {
            var type = typeof(IEntitySet<TEntity>);
            IEntitySet<TEntity> result;
            if (!_proxyMap.ContainsKey(type))
            {
                result = new EntitySetProxy<TEntity>(_provider.Set<TEntity>());
                _proxyMap[type] = () => result;
            }
            else
            {
                result = (IEntitySet<TEntity>)_proxyMap[type]();
            }
            return result;
        }
    }
}