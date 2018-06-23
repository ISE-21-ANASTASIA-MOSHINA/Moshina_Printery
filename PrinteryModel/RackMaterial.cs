using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractPrinteryModel
{
    public class RackMaterial // сколько материала на складе
    {
        [Key]
        public int Namber { get; set; }

        public int RackNamber { get; set; }

        public int MaterialNamber { get; set; }

        public int Count { get; set; }
        
        public virtual Rack Rack { get; set; }

        public virtual Material Material { get; set; }
    }
}
