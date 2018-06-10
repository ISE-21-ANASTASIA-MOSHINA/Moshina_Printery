using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrinterySVC.BindingModel;
using PrinterySVC.ViewModel;


namespace PrinterySVC.Inteface
{
    public interface ICustomerSVC
    {

        List<CustomerVievModel> GetList();
        CustomerVievModel GetElement(int number);
        void AddElement(CustomerBindingModel model);
        void UpElement(CustomerBindingModel model);
        void DelElement(int number);
    }
}
