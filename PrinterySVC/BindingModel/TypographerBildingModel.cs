using System.Runtime.Serialization;

namespace PrinterySVC.BindingModel
{
    [DataContract]
    public class TypographerBildingModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string TypographerFIO { get; set; }
    }
}
