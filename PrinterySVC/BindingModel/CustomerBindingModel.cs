using System.Runtime.Serialization;

namespace PrinterySVC.BindingModel
{
    [DataContract]
    public class CustomerBindingModel
    {
        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public string CustomerFIO { get; set; }
    }
}
