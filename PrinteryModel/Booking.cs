using System;
using System.ComponentModel.DataAnnotations;

namespace PrinteryModel
{
    public class Booking
    {
        [Key]
        public int Number { get; set; }
        public int CustomerNumber { get; set; }
        public int EditionNumber { get; set; }
        public int? TypographerNumber { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateTypographer { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Edition Edition { get; set; }
        public virtual Typographer Typographer { get; set; }
    }
}
