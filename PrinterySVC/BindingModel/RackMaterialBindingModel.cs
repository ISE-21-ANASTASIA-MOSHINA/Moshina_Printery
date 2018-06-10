using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.BindingModel
{
    [DataContract]

    public class RackMaterialBindingModel
    {
        [DataMember]
        public int Namber { get; set; }
        [DataMember]
        public int RackNamber { get; set; }
        [DataMember]
        public int MaterialNamber { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
