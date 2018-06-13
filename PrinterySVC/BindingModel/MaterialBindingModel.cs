using System.Runtime.Serialization;

namespace PrinterySVC.BindingModel
{
    [DataContract]
    public class MaterialBindingModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string MaterialName { get; set; }
    }
}
