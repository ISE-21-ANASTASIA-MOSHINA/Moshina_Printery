using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinteryModel
{
    public class Customer
    {
        [Key]
        public int Number { get; set; }

        [Required]
        public string CustomerFIO { get; set; }

        [ForeignKey("CustomerNumber")]
        public virtual List<Booking> Bookings { get; set; }
    }
}
