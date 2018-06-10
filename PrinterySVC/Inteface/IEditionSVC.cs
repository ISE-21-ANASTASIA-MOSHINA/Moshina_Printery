using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;
using System.Collections.Generic;

namespace PrinterySVC.Inteface
{
    public interface IEditionSVC
    {
        List<EditionViewModel> GetList();
        EditionViewModel GetElement(int number);
        void AddElement(EditionBindingModel model);
        void UpElement(EditionBindingModel model);
        void DelElement(int number);
    }
}
