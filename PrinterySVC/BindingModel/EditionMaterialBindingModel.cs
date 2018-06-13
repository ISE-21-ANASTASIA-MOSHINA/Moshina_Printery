using System.Runtime.Serialization;

namespace PrinterySVC.BindingModel
{
    [DataContract]
    public class EditionMaterialBindingModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int EditionNumber { get; set; }
        [DataMember]
        public int MaterialNumber { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
