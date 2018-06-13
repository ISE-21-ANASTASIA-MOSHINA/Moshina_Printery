using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class RackViewModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string RackName { get; set; }
        [DataMember]
        public List <RackMaterialViewModel> RackMaterials { get; set; }
    }
}
