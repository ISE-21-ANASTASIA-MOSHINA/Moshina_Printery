using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.BindingModel
{
    [DataContract]
    public class RackBindingModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string RackName { get; set; }
    }
}
