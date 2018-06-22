using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.ViewModel
{
    public class BookingViewModel
    {
        public int Number { get; set; }
        public int CustomerNumber { get; set; }
        public string CustomerFIO { get; set; }
        public int EditionNumber { get; set; }
        public string EditionName { get; set; }
        public int? TypographerNumber { get; set; }
        public string TypographerName { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public string Status { get; set; }
        public string DateCreate { get; set; }
        public string DateTypograph{ get; set; }
    }
}
