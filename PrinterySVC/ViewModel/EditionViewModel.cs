using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class EditionViewModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string EditionName { get; set; }
        [DataMember]
        public decimal Coast { get; set; }
        [DataMember]
        public List<EditionMaterialViewModel> EditionMaterials { get; set; }
    }
}
