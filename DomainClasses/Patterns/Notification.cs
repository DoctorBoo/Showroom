using DomainClasses.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses.Helpers
{
    /// <summary>
    /// Used for observer-pattern using dynamic-style/JSON. Serialize with telerik or ducktyped libraries.
    /// It is a observable and has singleton properties for supporting global signaling the semaphore/monitor.
    /// A provider or subject, which is the object that sends notifications to observers.
    /// Each notification is related to a systemwide notification provider.
    /// </summary>
    public class Notification: Subject
    {
        /// <summary>
        /// A singleton provider of notifications. Subscribe to it when notifications are needed systemwide.
        /// </summary>
        public static Notification Provider
        {
            get { return Singleton<Notification>.UniqueInstance; }
        }
        /// <summary>
        /// Use a locker for signaling the semaphore/monitor.
        /// Singleton-style.
        /// </summary>
        public static object Locker
        {
            get { return Singleton<object>.UniqueInstance; }
        }

        public static ConcurrentDictionary<string, Notification> Dictionary
        {
            get { return Singleton<ConcurrentDictionary<string, Notification>>.UniqueInstance; }
        }

        public static IList<IObserver<Notification>> Observers
        {
            get { return Singleton<List<IObserver<Notification>>>.UniqueInstance; }
        }
        public bool IsBusy { get; set; }

        public static bool IsLocked
        {
            get { return Singleton<Notification>.UniqueInstance.IsBusy; }
            set { Singleton<Notification>.UniqueInstance.IsBusy = value; }
        }
        public IDictionary<string, object> Messages { get; set; }

        public Notification()
        {
            Messages = new Dictionary<string, object>();
        }
        public static void Log(string key, object subject)
        {

        }
    }
}
