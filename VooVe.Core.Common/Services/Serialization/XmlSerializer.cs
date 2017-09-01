using System.IO;

namespace VooVe.Core.Common.Services.Serialization
{
    public class XmlSerializer : ISerializer
    {
        public T Get<T>(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    return (T) serializer.Deserialize(stream);
                }
            }
            catch
            {
                return default(T);
            }
        }

        public void Set<T>(string path, T item)
        {
            using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                serializer.Serialize(stream, item);
            }
        }
    }
}