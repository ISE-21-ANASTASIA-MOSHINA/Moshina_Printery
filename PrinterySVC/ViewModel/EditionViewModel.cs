using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.ViewModel
{
    public class EditionViewModel
    {
        public int Number { get; set; }
        public string EditionName { get; set; }
        public decimal Coast { get; set; }
        public List<EditionMaterialViewModel> EditionMaterials { get; set; }
    }
}
