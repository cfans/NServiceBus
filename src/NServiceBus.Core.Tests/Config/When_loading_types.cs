namespace NServiceBus.Core.Tests.Config
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NUnit.Framework;

    [TestFixture]
    public class When_loading_types
    {
        private List<Type> loadedTypes;

        [SetUp]
        public void SetUp()
        {
            var configure = Configure.With(Assembly.GetExecutingAssembly());
            loadedTypes = configure.TypesToScan.ToList();
        }

        [Test]
        public void Should_exclude_the_raven_client_types()
        {
            CollectionAssert.AreEquivalent(
                new Type[0],
                loadedTypes.Where(a => a.Namespace != null && a.Namespace.StartsWith("Raven.Client")).ToArray());
        }

        [Test]
        public void Should_always_include_the_core_nservicebus_types()
        {
            Assert.True(
                loadedTypes.Any(a => a.Assembly.GetName().Name.Equals("NServiceBus.Core")));
        }
    }

    public class TestClass
    {
        
    }
}