using System.Collections.Generic;
using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;


namespace PrinterySVC.Inteface
{
    public interface ICustomerSVC
    {
        List<CustomerVievModel> GetList();
        CustomerVievModel GetElement(int number);
        void AddElement(CustomerBindingModel model);
        void UpdElement(CustomerBindingModel model);
        void DelElement(int number);
    }
}
