using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterySVC.Inteface
{
    public interface IEditionSVC
    {
        List<EditionViewModel> GetList();
        EditionViewModel GetElement(int number);
        void AddElement(EdiitionBindingModel model);
        void UpElement(EdiitionBindingModel model);
        void DelElement(int number);
    }
}
