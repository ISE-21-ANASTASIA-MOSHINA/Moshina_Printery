using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.BindingModel
{
    public class EdiitionViewModel
    {
        public int Number { get; set; }

        public string EditionName { get; set; }

        public decimal Coast { get; set; }

        public List<EditionMaterialBindingModel> EditionMaterials { get; set; }
    }
}
