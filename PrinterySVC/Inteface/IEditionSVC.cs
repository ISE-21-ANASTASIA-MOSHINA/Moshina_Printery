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
        void AddElement(EditionBindingModel model);
        void UpdElement(EditionBindingModel model);
        void DelElement(int number);
    }
}
