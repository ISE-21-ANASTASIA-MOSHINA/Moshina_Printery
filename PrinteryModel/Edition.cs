using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractPrinteryModel
{
    public class Edition
    {
        [Key]
        public int Number { get; set; }

        [Required]
        public string EditionName { get; set; }

        [Required]
        public decimal CostEdition { get; set; }

        [ForeignKey("EditionNumber")]
        public virtual List<Booking> Bookings { get; set; }

        [ForeignKey("EditionNamber")]
        public virtual List<EditionMaterial> EditionMaterials { get; set; }
    
}
}
