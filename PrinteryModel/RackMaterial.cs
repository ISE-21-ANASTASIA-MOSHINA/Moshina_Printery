using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinteryModel
{
    public class RackMaterial // сколько материала на складе
    {
        [Key]
        public int Number { get; set; }

        public int RackNumber { get; set; }

        public int MaterialNumber { get; set; }

        public int Count { get; set; }

        public virtual Rack Rack { get; set; }

        public virtual Material Material { get; set; }
    }
}
