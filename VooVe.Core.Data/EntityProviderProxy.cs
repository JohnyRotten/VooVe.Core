namespace VooVe.Core.Data
{
    public class EntityProviderProxy : IEntityProvider
    {
        public IEntitySet<TEntity> Set<TEntity>()
        {
            throw new System.NotImplementedException();
        }
    }
}