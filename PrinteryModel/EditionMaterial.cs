using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinteryModel
{
    public class EditionMaterial
    {
        // сколько материала для изделия

        [Key]
        public int Number { get; set; }

        public int EditionNumber { get; set; }

        public int MaterialNumber { get; set; }

        public int Count { get; set; }

        public virtual Edition Edition { get; set; }

        public virtual Material Material { get; set; }

    }
}
