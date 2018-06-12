using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrinteryModel
{
    public class Typographer
    {
        [Key]
        public int Number { get; set; }

        [Required]
        public string TypographerFIO { get; set; }

        [ForeignKey("TypographerNumber")]
        public virtual List<Booking> Bookings { get; set; }
    }
}
