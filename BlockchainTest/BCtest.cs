using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Helpers;
using Repository.Entities;

namespace BlockchainTest
{
    [TestClass]
    public class BCtest
    {
        protected ResourceManager _rm;
        private string _tempKey = Guid.NewGuid().ToString();
        private static TestContext _testContext;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log<BCtest>.Write.Info("BCtest initialized.");
            _testContext = testContext;
        }

        [TestInitialize()]
        public void Initialize()
        {
            Log<BCtest>.Write.Info(String.Format("TestMethodInit {0}", _testContext.TestName));
            _rm = new ResourceManager();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            Log<BCtest>.Write.Info(String.Format("TestMethodCleanup {0}", _testContext.TestName));
            _rm.Mine.Delete(m => m.Origin == _tempKey);
            _rm.Mine.SaveChanges();
            _rm.Dispose();
        }
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Log<BCtest>.Write.Info(String.Format("ClassCleanup {0}", _testContext.TestName));
        }
        [TestMethod]
        public void AddTest()
        {
            string name = "eth";
            Mine item = new Mine() { Name = name, Value = 10.4m, Origin = _tempKey, CreationDate = DateTime.Now };
            
            _rm.Mine.Create(item);
            _rm.Mine.SaveChanges();

            var found = _rm.Mine.FindOne(f => f.Name == name);

            Assert.AreEqual(name, found.Name);
        }
        [TestMethod]
        public void RemoveTest()
        {
        }
    }
}
