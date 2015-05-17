using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMail
{
    class Program
    {
        object locker = new object();
        private static string token;
        private static bool mailSent;

        static void Main(string[] args)
        {
            int length = int.Parse(args[0]);
            new Program().StressTest(length);
        }
        [ContractInvariantMethod]
        public void StressTest(int length)
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
                                        try
                                        {
                                            String expectedToken = Guid.NewGuid().ToString();
                                            int month = rnd.Next(1, 12);
                                            int day = month == 2 ? rnd.Next(1, 28) : rnd.Next(1, 30);

                                            factory.SendCalendarEvent("dmodiwirijo@hotmail.com", "STRESSTEST" + iCurrent, null, new DateTime(2015, month, day), expectedToken);
                                            string actual;
                                            OfficeFactory._tokens.TryGetValue(expectedToken, out actual);

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
    }
}
