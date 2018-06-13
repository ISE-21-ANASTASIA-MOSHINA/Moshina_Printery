using System.Runtime.Serialization;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class MaterialViewModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string MaterialName { get; set; }
    }
}
