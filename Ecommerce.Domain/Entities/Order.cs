namespace Ecommerce.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Order", Schema = "dbo")]
    public class Order
    {
        public Order()
        {
            OrderDetails = [];
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(100)]
        public string CustomerEmail { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal Total { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
