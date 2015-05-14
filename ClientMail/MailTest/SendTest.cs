using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Threading;
using ClientMail;
using Microsoft.Office.Interop.Outlook;

namespace TestRepo
{
    /// <summary>
    /// Summary description for Mail
    /// </summary>
    [TestClass]
    public class Mail
    {
        object locker = new object();
        public Mail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;
        private static string token;
        private static bool mailSent;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {

        }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMail()
        {
            lock (locker)
            {
                String expectedToken = Guid.NewGuid().ToString();

                OfficeFactory factory = new OfficeFactory();
                factory.Send("smtp.gmail.com", 587, "sygnion", "sygnion01!", expectedToken);
                //Monitor.Wait(locker);

                string actual;
                OfficeFactory._tokens.TryGetValue(expectedToken, out actual);
                Assert.AreEqual(expectedToken, actual);
            }
        }
        public void TestAppointment()
        {
            OfficeFactory office = new OfficeFactory();
            
        }
    }
}
