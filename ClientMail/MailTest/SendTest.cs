using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Threading;
using ClientMail;
using System.Collections.Concurrent;
using System.Threading.Tasks;

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
        private readonly int length = 25;


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

                OfficeFactory factory = new OfficeFactory("smtp.gmail.com", 587, "sygnion2", "sygnion01!");
                factory.SendCalendarEvent("dmodiwirijo@hotmail.com", " Dwight Modiwirijo | YMultego", null, new DateTime(2015, 7, 7), expectedToken);
                //Monitor.Wait(locker);

                string actual;
                OfficeFactory._tokens.TryGetValue(expectedToken, out actual);
                Assert.AreEqual(expectedToken, actual);
            }
        }
        [TestMethod]
        public void StressTest()
        {
            Action[] jobQueue = new Action[4]; 
            ConcurrentQueue<Exception> errors = new ConcurrentQueue<Exception>();
            for (int i = 0; i < 4; i++)
            {
                jobQueue[i] = () =>
                {
                    using (OfficeFactory factory = new OfficeFactory("smtp.gmail.com", 587, "sygnion2", "sygnion01!"))
                    {
                        ConcurrentBag<Task> tasks = new ConcurrentBag<Task>();
                        ConcurrentBag<Action<string, DateTime, string>> bag = new ConcurrentBag<Action<string, DateTime, string>>();

                        Random rnd = new Random();
                        Action[] actionList = new Action[length];

                        //Create jobs
                        for (int j = 0; j < length; j++)
                        {
                            Action action = () =>
                            {
                                lock (locker)
                                {
                                    {
                                        try
                                        {
                                            String expectedToken = Guid.NewGuid().ToString();
                                            int month = rnd.Next(1, 12);
                                            int day = month == 2 ? rnd.Next(1, 28) : rnd.Next(1, 30);
                                            factory.SendCalendarEvent("dmodiwirijo@hotmail.com", "STRESSTEST", null, new DateTime(2015, month, day), expectedToken);
                                            string actual;
                                            OfficeFactory._tokens.TryGetValue(expectedToken, out actual);
                                            Assert.AreEqual(expectedToken, actual);
                                        }
                                        catch (Exception ex)
                                        {
                                            errors.Enqueue(ex);
                                        }
                                    }
                                }
                            };
                            actionList[j] = action;
                        }

                        //Execute jobs parallel
                        ParallelLoopResult result = Parallel.For(0, length, ctr =>
                        {
                            var task = Task.Factory.StartNew(actionList[ctr]);
                            tasks.Add(task);
                        });

                        //Wait on all finished
                        Task.WaitAll(tasks.ToArray());
                        foreach (var failure in OfficeFactory._failure)
                        {
                            Console.WriteLine("[{0}]\r\n{1}", failure.Key, failure.Value);
                        }

                        foreach (var error in errors)
                        {
                            Console.WriteLine("[{0}]\r\n{1}", error.Message, error.InnerException != null ? error.InnerException.Message : null);
                        }
                    }
                };
            };

            ConcurrentBag<Task> tasks2 = new ConcurrentBag<Task>();
            foreach (var job in jobQueue)
            {
                var task = Task.Factory.StartNew(job);
                tasks2.Add(task);
            }
            Task.WaitAll(tasks2.ToArray());
        }
    }
}
