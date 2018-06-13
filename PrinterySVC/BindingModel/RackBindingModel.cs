using System.Runtime.Serialization;

namespace PrinterySVC.BindingModel
{
    [DataContract]
    public class RackBindingModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string RackName { get; set; }
    }
}
