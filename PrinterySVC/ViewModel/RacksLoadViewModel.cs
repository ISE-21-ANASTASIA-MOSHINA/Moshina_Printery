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
        public List<RacksMaterailLoadViewModel> Materials { get; set; }
    }

        [DataContract]
        public class RacksMaterailLoadViewModel
        {
            [DataMember]
            public string MaterialName { get; set; }

            [DataMember]
            public int Count { get; set; }

        }
    
}
