using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.BindingModel
{
    public class BookingBindingModel
    {
        public int Number { get; set; }
        public int CustomerNumber { get; set; }
        public int EditionNumber { get; set; }
        public int? TypographerNumber { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}
