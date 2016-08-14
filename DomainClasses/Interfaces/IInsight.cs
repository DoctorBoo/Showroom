
using DomainClasses.Helpers;
using System;

namespace DomainClasses.Interfaces
{
    public interface IInsight
    {
        void OnCompleted();
        void OnError(Exception e);
        void OnNext(Notification notification);
        void Subscribe(IObservable<Notification> provider);
        void Unsubscribe();
    }
}
