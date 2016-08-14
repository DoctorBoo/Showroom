using System;
using System.Security.Principal;
namespace DomainClasses.Interfaces
{
    public abstract class Identity : IIdentity
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public string AuthenticationType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAuthenticated
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }
    }
}
