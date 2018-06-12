using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;

namespace PrinterySVC.Inteface
{
    public interface IMaterialSVC
    {
        List<MaterialViewModel> GetList();
        MaterialViewModel GetElement(int number);
        void AddElement(MaterialBindingModel model);
        void UpdElement(MaterialBindingModel model);
        void DelElement(int number);
    }
}
