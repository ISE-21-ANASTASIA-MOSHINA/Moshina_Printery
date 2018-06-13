using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractPrinteryModel
{
    public class Rack //Стелаж
    {
        [Key]
        public int Number { get; set; }

        [Required]
        public string RackName { get; set; }

        [ForeignKey("RackNamber")]
        public virtual List<RackMaterial> RackMaterials { get; set; }

    }
}
