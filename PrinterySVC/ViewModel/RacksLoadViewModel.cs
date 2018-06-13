using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class RacksLoadViewModel
    {
        [DataMember]
        public string RackName { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public IEnumerable<Tuple<string, int>> Materials { get; set; }

    }
}
