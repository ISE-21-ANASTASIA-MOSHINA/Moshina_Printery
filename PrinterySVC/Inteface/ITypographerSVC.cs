using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.Inteface
{
    public interface ITypographerSVC
    {
        List<TypographerViewModel> GetList();
        TypographerViewModel GetElement(int number);
        void AddElement(TypographerBildingModel model);
        void UpElement(TypographerBildingModel model);
        void DelElement(int number);
    }
}
