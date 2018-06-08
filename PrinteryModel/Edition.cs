using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinteryModel
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

        [ForeignKey("EditionNumber")]
        public virtual List<EditionMaterial> EditionMaterials { get; set; }
    }
}
