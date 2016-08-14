using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainClasses.Interfaces
{
    public interface IMetaData
    {
        int IdIndex { get; set; }
        decimal IdWidth { get; set; }
        int ClientNameIndex { get; set; }
        decimal ClientNameWidth { get; set; }
        int LimitIndex { get; set; }
        decimal LimitWidth { get; set; }
        int SaldoIndex { get; set; }
        decimal SaldoWidth { get; set; }
        int PermilleIndex { get; set; }
        decimal PermilleWidth { get; set; }
        int ProvisionIndex { get; set; }
        decimal ProvisionWidth { get; set; }
        int BankIndex { get; set; }
        decimal BankWidth { get; set; }
        int BirthDateIndex { get; set; }
        decimal BirthDateWidth { get; set; }
        int PostalCodeIndex { get; set; }
        decimal PostalCodeWidth { get; set; }
        IDictionary<string, dynamic> Properties { get; set; }
        IDictionary<string, dynamic> CorrectionProperties { get; set; }
    }
}
