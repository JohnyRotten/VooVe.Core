namespace VooVe.Core.Common.Services.Serialization
{
    public interface ISerializer
    {
        T Get<T>(string  path);
        void Set<T>(string path, T item);
    }
}