using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using VooVe.Core.Common.SystemExtentions;

namespace VooVe.Core.Tests
{
    [TestFixture]
    public class IoCTests
    {

        private IoC _ioc;

        [SetUp]
        public void Initialize()
        {
            _ioc = new IoC();
        }

        [Test]
        public void ResolveTest()
        {
            _ioc.Bind<ITest, TestA>();
            Assert.IsInstanceOf<TestA>(_ioc.Resolve<ITest>());
            _ioc.Bind<ITest, TestB>();
            Assert.IsInstanceOf<TestB>(_ioc.Resolve<ITest>());
        }

        [Test]
        public void UnresolvedConstructorTest()
        {
            _ioc.Bind<TestC>();
            Assert.Catch<UnconstructedException>(() => _ioc.Resolve<TestC>());
        }

        [Test]
        public void CyclingLinksTest()
        {
            _ioc.Bind<ITest, TestC>();
            Assert.Catch<CyclingTypingException>(() => _ioc.Resolve<ITest>());
            _ioc.Bind<ITest, TestA>();
            _ioc.Bind<TestC>();
            Assert.IsNotInstanceOf<TestC>(_ioc.Resolve<ITest>());
            Assert.IsInstanceOf<TestC>(_ioc.Resolve<TestC>());
        }

        [Test]
        public void InstanceBindTest()
        {
            _ioc.BindInstance<ITest, TestB>();
            var first = _ioc.Resolve<ITest>();
            Assert.AreEqual(true, Enumerable.Range(0, 100).All(_ => first == _ioc.Resolve<ITest>()));
        }

        [Test]
        public void SingletonBindTest()
        {
            _ioc.BindSingleton<ITest, TestB>();
            var first = _ioc.Resolve<ITest>();
            Assert.AreEqual(true, Enumerable.Range(0, 100).All(_ => first == _ioc.Resolve<ITest>()));
        }

        [Test]
        public void MultipleParametersTest()
        {
            _ioc.Bind<TestD>();
            Assert.Catch<UnconstructedException>(() => _ioc.Resolve<TestD>());
            _ioc.Bind<ITest, TestB>();
            Assert.Catch<UnconstructedException>(() => _ioc.Resolve<TestD>());
            _ioc.Bind<TestA>();
            Assert.AreNotEqual(null, _ioc.Resolve<TestD>());
        }

        [Test]
        public void BindInstancedObjectTest()
        {
            var impl = new TestA();
            _ioc.BindInstance<ITest>(impl);
            Enumerable.Range(0, 100).ForEach(_ => Assert.AreEqual(impl, _ioc.Resolve<ITest>()));
        }

        [Test]
        public void FunctionBindTest()
        {
            _ioc.Bind<ITest>(() => new TestA());
            var impl = _ioc.Resolve<ITest>();
            Enumerable.Range(0, 100).ForEach(_ => Assert.AreNotEqual(impl, _ioc.Resolve<ITest>()));
        }

        #region Test classes

        private interface ITest
        {
            int Foo();
        }

        private class TestA : ITest
        {
            public int Foo() => 1;
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestB : ITest
        {
            public int Foo() => 2;
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestC : ITest
        {
            public int Foo() => 3;
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            private ITest Test { get; }
            public TestC(ITest test)
            {
                Test = test;
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private class TestD
        {
            public TestD(ITest test, TestA a)
            {
                
            }
        }

        #endregion

    }
}
