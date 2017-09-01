using System;
using System.Linq;
using NUnit.Framework;
using VooVe.Core.Common.Services.Serialization;

namespace VooVe.Core.Common.Tests
{
    [TestFixture]
    public class SerializerTests
    {
        [TestCase(typeof(XmlSerializer))]
        public void SerializeTest(Type serializerType)
        {
            var serializer = (ISerializer)serializerType.GetConstructors()
                .First()
                .Invoke(new object[]{});
            var original = new SerializableClass { Id = 2 };
            const string path = @".\file.out";
            serializer.Set(path, original);
            var created = serializer.Get<SerializableClass>(path);
            Assert.AreEqual(original, created);
        }

        #region Test class

        [Serializable]
        public class SerializableClass
        {
            public int Id { get; set; }
            public override int GetHashCode() => ToString().GetHashCode();
            public override bool Equals(object obj) => obj is SerializableClass o && o.Id == Id;
        }

        #endregion
    }
}