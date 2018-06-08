using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinteryModel
{
    public class Rack //Стелаж
    {
        [Key]
        public int Number { get; set; }

        [Required]
        public string RackName { get; set; }

        [ForeignKey("RackNumber")]
        public virtual List<RackMaterial> RackMaterials { get; set; }
    }
}
