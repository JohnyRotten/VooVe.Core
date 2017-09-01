using NUnit.Framework;

namespace VooVe.Core.Data.Tests
{
    [TestFixture]
    public class EntityProviderTests
    {
        private IEntityProvider _provider;

        [SetUp]
        [TestCase]
        public void InitializeTest<TProvider>() where TProvider : IEntityProvider, new()
        {
            _provider = new TProvider();
        }

        [Test]
        public void GetItemsTest()
        {
            var set = _provider.Set<IIdentable>();
            var item = new Idable { Id = 1 };
            var good = new Good { Id = 2, Name = "Good", Price = 42m };
            set.Add(item);
            set.Add(good);
        }

        #region Test entities

        private interface IIdentable
        {
            int Id { get; set; }
        }

        private interface IPricable
        {
            decimal Price { get; set; }
        }

        private class Good : IIdentable, IPricable
        {
            public int Id { get; set; }
            public decimal Price { get; set; }
            public string Name { get; set; }
        }

        private class Idable : IIdentable
        {
            public int Id { get; set; }
        }

        private class Pricable : IPricable
        {
            public decimal Price { get; set; }
        }

        #endregion
    }
}