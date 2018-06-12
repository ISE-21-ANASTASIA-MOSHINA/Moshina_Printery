using System.ComponentModel.DataAnnotations;

namespace PrinteryModel
{
    public class EditionMaterial
    {
        [Key]
        public int Number { get; set; }
        public int EditionNumber { get; set; }
        public int MaterialNumber { get; set; }
        public int Count { get; set; }
        public virtual Edition Edition { get; set; }
        public virtual Material Material { get; set; }
    }
}
