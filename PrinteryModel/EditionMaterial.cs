

using System.ComponentModel.DataAnnotations;

namespace AbstractPrinteryModel
{
    public class EditionMaterial
    {
        // сколько материала для изделия
        [Key]
        public int Number { get; set; }

        public int EditionNamber { get; set; }

        public int MaterialNamber { get; set; }

        public int Count { get; set; }

        public virtual Edition Edition { get; set; }

        public virtual Material Material { get; set; }
    }
}
