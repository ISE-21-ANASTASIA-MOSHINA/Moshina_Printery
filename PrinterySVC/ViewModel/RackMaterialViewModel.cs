using System.Runtime.Serialization;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class RackMaterialViewModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int RackNumber { get; set; }
        [DataMember]
        public int MaterialNumber { get; set; }
        [DataMember]
        public string MaterialName { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
