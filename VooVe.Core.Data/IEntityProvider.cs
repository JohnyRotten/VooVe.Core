namespace VooVe.Core.Data
{
    public interface IEntityProvider
    {
        IEntitySet<TEntity> Set<TEntity>();
    }
}