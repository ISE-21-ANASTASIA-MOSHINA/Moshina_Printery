using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class CustomerVievModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string CustomerFIO { get; set; }
    }
}
