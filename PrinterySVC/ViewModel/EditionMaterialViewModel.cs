﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.ViewModel
{
    [DataContract]
    public class EditionMaterialViewModel
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int EditionNamber { get; set; }
        [DataMember]
        public int MaterialNamber { get; set; }
        [DataMember]
        public string MaterialName { get; set; }
        [DataMember]
        public int Count { get; set; }
        
    }
}
