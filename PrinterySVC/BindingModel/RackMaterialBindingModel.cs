using System.Runtime.Serialization;

namespace PrinterySVC.BindingModel
{
    [DataContract]
    public class RackMaterialBindingModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int RackNumber { get; set; }
        [DataMember]
        public int MaterialNumber { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
