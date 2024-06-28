namespace Ecommerce.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    [Table("Order", Schema = "dbo")]
    public class Order
    {
        public Order()
        {
            this.OrderDetails = [];
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        [StringLength(100)]
        public string CustomerName { get; set; }

        [StringLength(100)]
        public string CustomerEmail { get; set; }

        public DateTime OrderDate { get; set; }

        [StringLength(50)]
        public  string CustomerEmail { get; set; }
        public  DateTime OrderDate { get; set; }
        public decimal Total { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
