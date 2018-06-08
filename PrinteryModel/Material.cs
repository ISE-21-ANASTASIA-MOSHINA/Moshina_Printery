using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinteryModel
{
    public class Material
    {
        [Key]
        public int Number { get; set; }

        [Required]
        public string MaterialName { get; set; }

        [ForeignKey("MaterialNumber")]
        public virtual List<EditionMaterial> EditionMaterials { get; set; }

        [ForeignKey("MaterialNumber")]
        public virtual List<RackMaterial> RackMaterials { get; set; }
    }
}
