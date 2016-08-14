using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses.Interfaces
{
    public interface IEntity: IType
    {
        string Email { get; set; }
        string Address { get; set; }
        string Zipcode { get; set; }
        string Telephone { get; set; }
        string City { get; set; }
        string ChaimberOfCommerce { get; set; }
        string AFM { get; set; }
        string BankAccount { get; set; }
    }
}
