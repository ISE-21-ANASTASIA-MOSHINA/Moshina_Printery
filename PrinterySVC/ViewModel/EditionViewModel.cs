using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
        public decimal Cost { get; set; }
        [DataMember]
        public List<EditionMaterialViewModel> EditionMaterials { get; set; }
    }
}
