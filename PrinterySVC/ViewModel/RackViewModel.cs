﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.ViewModel
{
    public class RackViewModel
    {
        public int Number { get; set; }
        public string RackName { get; set; }
        public List <RackMaterialViewModel> RackMaterials { get; set; }
    }
}
