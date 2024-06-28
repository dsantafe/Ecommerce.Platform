using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public  int OrderID { get; set; }

        [StringLength(50)]
        public  string CustomerName { get; set; }

        [StringLength(50)]
        public  string CustomerEmail { get; set; }
        public  DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
    }
}
