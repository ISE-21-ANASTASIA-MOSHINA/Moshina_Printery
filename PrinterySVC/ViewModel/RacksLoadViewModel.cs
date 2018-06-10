using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.ViewModel
{
    public class RacksLoadViewModel
    {
        public string RackName { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<Tuple<string, int>> Materials { get; set; }

    }
}
