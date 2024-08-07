using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Services.Contracts
{
    public interface IInvoiceService
    {
        Task CreateInvoice(int orderId);
    }
}
