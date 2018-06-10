using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.ViewModel
{

    [DataContract]
    public class RackMaterialViewModel
    {
        [DataMember]
        public int Namber { get; set; }
        [DataMember]
        public int RackNamber { get; set; }
        [DataMember]
        public int MaterialNameber { get; set; }
        [DataMember]
        public string MaterialName { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
