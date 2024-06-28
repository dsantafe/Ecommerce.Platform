﻿namespace Ecommerce.Domain.Entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }

        #region ..::Virtual::..
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        #endregion
    }
}
