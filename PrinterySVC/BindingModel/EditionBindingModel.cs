using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PrinterySVC.BindingModel
{
    [DataContract]
    public class EditionBindingModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string EditionName { get; set; }
        [DataMember]
        public decimal Coast { get; set; }
        [DataMember]
        public List<EditionMaterialBindingModel> EditionMaterials { get; set; }
    }
}
