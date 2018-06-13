using System.Runtime.Serialization;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class TypographerViewModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string TypographerFIO { get; set; }
    }
}
