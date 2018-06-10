using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.Inteface
{
    public interface IRackSVC
    {
        List<RackViewModel> GetList();
        RackViewModel GetElement(int number);
        void AddElement(RackBindingModel model);
        void UpElement(RackBindingModel model);
        void DelElement(int number);
    }
}
