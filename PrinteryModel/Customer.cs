
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AbstractPrinteryModel
{
    public class Customer
    {
        [Key]
        public int Number { get; set; }

        [Required]
        public string CustomerFIO { get; set; }

        [ForeignKey("CustomerNumber")]
        public virtual List<Booking> Bookings {get; set;}
    }
}
