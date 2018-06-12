using System.ComponentModel.DataAnnotations;

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
