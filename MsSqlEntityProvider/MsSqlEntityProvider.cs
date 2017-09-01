using VooVe.Core.Data;

namespace MsSqlEntityProvider
{
    public class MsSqlEntityProvider : IEntityProvider
    {
        public MsSqlEntityProvider(string connectionString)
        {
            
        }

        public IEntitySet<TEntity> Set<TEntity>()
        {
            throw new System.NotImplementedException();
        }
    }
}