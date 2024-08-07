using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Entities
{
    public class Invoice:BaseEntity
    {
        public DateTime InvoiceDate { get; set; }
        public decimal Totalamount { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }

    }
}
