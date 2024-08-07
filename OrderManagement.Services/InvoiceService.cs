using OrderManagement.Core;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Services.Contracts;
using OrderManagement.Core.Specifications.OrderSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateInvoice(int orderId)
        {
            var spec = new OrdersWithItemsSpecification(orderId);
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            var totalAmount = order.Totalamount;
            var invoice = new Invoice
            {
                OrderId=order.Id,
                InvoiceDate=DateTime.Now,
                Totalamount=totalAmount,
            };
            await _unitOfWork.Repository<Invoice>().AddAsync(invoice);
            await _unitOfWork.CompleteAsync();
        }
    }
}
