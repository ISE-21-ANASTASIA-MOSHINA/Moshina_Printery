using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
        public decimal Price { get; set; }

        [DataMember]
        public List<EditionMaterialBindingModel> EditionMaterials { get; set; }
    }
}

