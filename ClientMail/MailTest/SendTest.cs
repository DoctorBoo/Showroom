using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Threading;
using ClientMail;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace TestRepo
{
    /// <summary>
    /// Summary description for Mail
    /// </summary>
    [TestClass]
    public class Mail
    {
        private TestContext testContextInstance;
        object locker = new object();
        private static string token;
        private static bool mailSent;
        private readonly int length = 2;
        public Mail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
                DateTime date = new DateTime(2015, 1, 7);
                factory.SendCalendarEvent("dmodiwirijo@hotmail.com", " Dwight Modiwirijo | Winter inc.", null, date, expectedToken);
                date = new DateTime(2015, 7, 7);
                factory.SendCalendarEvent("dmodiwirijo@hotmail.com", " Dwight Modiwirijo | Zomer Inc.", null, date, expectedToken);
                //Monitor.Wait(locker);

                string actual;
                OfficeFactory._tokens.TryGetValue(expectedToken, out actual);
                Assert.AreEqual(expectedToken, actual);
            }
        }
        [ContractInvariantMethod]
        [Ignore]
        public void StressTest()
        {
            Action[] jobQueue = new Action[4];
            Action[][] actionList = new Action[4][];
            
            ConcurrentQueue<Exception> errors = new ConcurrentQueue<Exception>();
            for (int i = 0; i < 4; i++)
            {
                int iCurrent = i;
                actionList[iCurrent] = new Action[length];                
                jobQueue[iCurrent] = () =>
                {
                    using (OfficeFactory factory = new OfficeFactory("smtp.gmail.com", 587, "sygnion2", "sygnion01!"))
                    {
                        ConcurrentBag<Task> tasks = new ConcurrentBag<Task>();
                        ConcurrentBag<Action<string, DateTime, string>> bag = new ConcurrentBag<Action<string, DateTime, string>>();

                        Random rnd = new Random();
                        
                        //Create jobs
                        for (int j = 0; j < length; j++)
                        {
                            Action action = () =>
                            {
                                lock (locker)
                                {
                                    {
                                        Contract.ContractFailed += Contract_ContractFailed;
                                        try
                                        {
                                            String expectedToken = Guid.NewGuid().ToString();
                                            int month = rnd.Next(1, 12);
                                            int day = month == 2 ? rnd.Next(1, 28) : rnd.Next(1, 30);
                                            
                                            factory.SendCalendarEvent("dmodiwirijo@hotmail.com", "STRESSTEST" + iCurrent, null, new DateTime(2015, month, day), expectedToken);
                                            string actual;
                                            OfficeFactory._tokens.TryGetValue(expectedToken, out actual);
                                            Assert.AreEqual(expectedToken, actual);
                                            Contract.Invariant(false);
                                        }
                                        catch (Exception ex)
                                        {
                                            errors.Enqueue(ex);
                                        }
                                    }
                                }
                            };
                            actionList[iCurrent][j] = action;
                        }

                        //Execute jobs parallel
                        ParallelLoopResult result = Parallel.For(0, length, ctr =>
                        {
                            var task = Task.Factory.StartNew(actionList[iCurrent][ctr]);
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
                    GC.WaitForPendingFinalizers();
                };
            };

            ConcurrentBag<Task> tasks2 = new ConcurrentBag<Task>();
            foreach (var job in jobQueue)
            {
                var task = Task.Factory.StartNew(job);
                tasks2.Add(task);
                Task.WaitAll(tasks2.ToArray());
                GC.WaitForPendingFinalizers();
            }
            //Task.WaitAll(tasks2.ToArray());
        }

        public static int  RollDice(Random rng)
        {
            return 0; 
        }
        private void Contract_ContractFailed(object sender, ContractFailedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
