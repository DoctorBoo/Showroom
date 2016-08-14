
using DomainClasses.Helpers;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DomainClasses.Interfaces
{
    public abstract class Observlet : Hub, IObserver<Notification>, IInsight
    {
        /// <summary>
        /// Systemwide provider of notifications.
        /// </summary>
        protected Notification _provider = Notification.Provider;
        public Action<string, object> Report { get; set; }
        protected ConcurrentDictionary<string, Notification> _notifications = Notification.Dictionary;
        protected IDisposable cancellation;

        #region observer property

        public virtual void Subscribe(IObservable<Notification> provider)
        {
            _provider = Notification.Provider;
            cancellation = provider.Subscribe(this);
        }

        public virtual void Unsubscribe()
        {
            cancellation.Dispose();
            _notifications.Clear();
        }

        #region iobserver
        public virtual void OnCompleted()
        {
            _notifications.Clear();
        }

        // No implementation needed: Method is not called by the BaggageHandler class.
        public virtual void OnError(Exception e)
        {
            // No implementation.
        }

        // Update information.
        public virtual void OnNext(Notification notification)
        {
            lock (Notification.Locker)
            {
                if (_notifications == null) _notifications = Notification.Dictionary;

                var keys = notification.Messages.Keys;
                foreach (var key in keys)
                {
                    if (!_notifications.ContainsKey(key))
                    {
                        object value;
                        notification.Messages.TryGetValue(key, out value);
                        _notifications.TryAdd(key, notification);

                        //Observer should implement/override a reporting feature.
                        if (Report != null) Report(key, value);
                        //DO NEVER Notification.Log(key, value);
                    }
                }
                Monitor.PulseAll(Notification.Locker);
                #if DEBUG
                    //Log<Observlet>.Write.Info("Monitor.PulseAll");
                #endif
            }
        }
        #endregion
        #endregion
    }
    /// <summary>
    /// An observer, which is an object that receives notifications from a provider.
    /// </summary>
    public abstract class Subject: IObservable<Notification>
    {
        /// <summary>
        /// Systemwide provider of notifications.
        /// </summary>
        protected Notification _provider = Notification.Provider;
        public Action<string, object> Report {get;set;}
        protected List<IObserver<Notification>> _observers = new List<IObserver<Notification>>();
        protected ConcurrentDictionary<string, Notification> _notifications = Notification.Dictionary;
        #region IObservable
        public IDisposable Subscribe(IObserver<Notification> observer)
        {
            lock(Notification.Locker)
            {                
                // Check whether observer is already registered. If not, add it
                if (!_observers.Contains(observer))
                {
                    _observers.Add(observer);
                    // Provide observer with existing data.
                    foreach (var item in _notifications)
                        observer.OnNext(item.Value);
                }

                _notifications.Clear();
                //Monitor.PulseAll(Notification.Locker);
                return new Unsubscriber<Notification>(_observers, observer);
            }
        }

        /// <summary>
        /// Notifies with Message dictionary asyncronously.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subject"></param>
        public virtual void NotifyObservers(string key, object subject)
        {
            //return;
            Task.Run(() =>
            {
                #region #if DEBUG
                //Do not use in production. It may consume too much resources.
                //Notification.Log(key, subject);
                #endregion #endif

                lock (Notification.Locker)
                {
                    //Observer should implement/override a reporting feature.
                    if (Report != null) Report(key, subject);

                    Notification notification = new Notification();
                    notification.Messages.Add(key, subject);
                    bool succeeded = _notifications.TryAdd(key, notification);

                    if (succeeded)
                    {
                        foreach (var observer in _observers)
                            observer.OnNext(notification);

                        if (_provider != null)
                            _provider.NotifyObservers(key, subject);
                        Monitor.PulseAll(Notification.Locker);
                    }
                }
            });

            //system-wide notifications
            //Task.Run(() =>
            //{
            //    if (_provider != null)
            //        _provider.NotifyObservers(subject);
            //});
        }
        #endregion
    }
}
