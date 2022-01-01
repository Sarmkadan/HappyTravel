using System;
using HappyTravel.Edo.Common.Enums;

namespace HappyTravel.Edo.Data.Suppliers
{
    public class SupplierOrder
    {
        public int Id { get; set; }
        public DataProviders DataProvider { get; set; }
        public decimal Price { get; set; }
        public SupplierOrderState State { get; set; }
        public ServiceTypes Type { get; set; }
        public string ReferenceCode { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}