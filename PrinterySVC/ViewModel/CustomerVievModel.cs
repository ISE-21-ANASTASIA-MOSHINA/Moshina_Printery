using System.Runtime.Serialization;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class CustomerVievModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string CustomerFIO { get; set; }
    }
}
