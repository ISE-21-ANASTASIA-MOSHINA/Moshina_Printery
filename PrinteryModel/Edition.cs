using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrinteryModel
{
    public class Edition
    {
        [Key]
        public int Number { get; set; }

        [Required]
        public string EditionName { get; set; }

        [Required]
        public decimal Coast { get; set; }

        [ForeignKey("EditionNumber")]
        public virtual List<Booking> Bookings { get; set; }

        [ForeignKey("EditionNumber")]
        public virtual List<EditionMaterial> EditionMaterials { get; set; }
    }
}
